using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using Photon.Realtime;

using PhotonRoomListGrpcService.Configs;
using PhotonRoomListGrpcService.Interfaces.IStorages;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PhotonRoomListGrpcService
{
    public class PhotonService : BackgroundService, IConnectionCallbacks, ILobbyCallbacks
    {
        private readonly string serviceName;

        private readonly ILogger<PhotonService> _logger;
        private readonly PhotonConfig photonConfig;
        private readonly IRoomList photonRoomListStorage;
        private readonly IAccountStorage accountStorage;

        private readonly LoadBalancingClient client = new();

        public PhotonService(ILogger<PhotonService> logger, IConfiguration phoConfig, IRoomList roomListStorage, IAccountStorage accStore)
        {
            serviceName = this.GetType().ToString();

            //Log
            _logger = logger;
            _logger.LogInformation("{serviceName} Start @{time}", serviceName, DateTimeOffset.Now);

            //Config
            photonConfig = new PhotonConfig();
            phoConfig.GetSection(PhotonConfig.Photon).Bind(photonConfig);

            //Storage
            photonRoomListStorage = roomListStorage;
            // Default TargetRegion come from Config
            photonRoomListStorage.TargetPhotonRegion = photonConfig.Region[0];

            //Account
            accountStorage = accStore;

            //Photon
            this.client.AddCallbackTarget(this);
            this.client.StateChanged += this.OnStateChange;

            this.client.AppId = photonConfig.AppId;
        }

        ~PhotonService()
        {
            _logger.LogInformation("{serviceName} End @{time}", serviceName, DateTimeOffset.Now);

            this.client.Disconnect();
            this.client.RemoveCallbackTarget(this);
        }

        void OnStateChange(ClientState arg1, ClientState arg2)
        {
            _logger.LogInformation($"PhotonStateChanges: {arg1} -> { arg2}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ExecuteAsync@{time} ", DateTimeOffset.Now);
            _ = AsyncLoop(stoppingToken);

            _ = TryConnectToMasterServer(stoppingToken);
        }

        async Task AsyncLoop(CancellationToken stoppingToken)
        {
            _logger.LogDebug("AsyncLoop Start@{time} ", DateTimeOffset.Now);
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    this.client.Service();
                    await Task.Delay(33, stoppingToken);

                    if (!this.client.IsConnectedAndReady || !this.client.InLobby)
                        continue;

                    // Only InLobby should Check target/current Region Difference
                    if (!photonRoomListStorage.IsRegionMatching())
                    {
                        _logger.LogWarning("AsyncLoop Disconnect@{time} for RegionChange", DateTimeOffset.Now);
                        this.client.Disconnect();
                    }
                }
                catch (OperationCanceledException)
                {
                    // Prevent throwing if the Delay is cancelled
                }
            }
            _logger.LogDebug("AsyncLoop End@{time} ", DateTimeOffset.Now);
        }

        TaskCompletionSource<bool> connectionTCS;
        async Task TryConnectToMasterServer(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (this.client.IsConnectedAndReady)
                {
                    _logger.LogDebug($"TryConnectToMasterServer AlreadyInServer {this.client.State}");
                    await Task.Delay(1000, stoppingToken);
                    continue;
                }

                var jwtString = "";
                if (!accountStorage.TryGetAuthInfo(out jwtString))
                {
                    _logger.LogDebug($"TryConnectToMasterServer NoAuthFound {this.client.State}");
                    await Task.Delay(1000, stoppingToken);
                    continue;
                }

                _logger.LogInformation("PhotonService.TryConnectToMasterServer @{time} ", DateTimeOffset.Now);

                _logger.LogDebug("TryConnectToMasterServer.TrySetAuthenticationValues");
                TrySetAuthenticationValues(jwtString);

                connectionTCS = new TaskCompletionSource<bool>();

                // ConnectToRegionMaster To photonRoomListStorage.TargetPhotonRegion
                this.client.ConnectToRegionMaster(photonRoomListStorage.TargetPhotonRegion);

                var cts = new CancellationTokenSource();
                cts.CancelAfter(10000);

                _logger.LogDebug("TryConnectToMasterServer Wait for ConnectToRegionMaster");
                //wait until connected or cancelled
                while (connectionTCS != null && !connectionTCS.Task.IsCompleted && !cts.IsCancellationRequested)
                {
                    try
                    {
                        await Task.Delay(100, cts.Token);
                    }
                    catch (TaskCanceledException)
                    {
                        _logger.LogWarning("TryConnectToMasterServer Cancelled");
                        connectionTCS.TrySetResult(false);

                        return;
                    }
                }

                await Task.Delay(3000, stoppingToken);
            }
        }

        bool TrySetAuthenticationValues(string secretData)
        {
            AuthenticationValues authValues = new AuthenticationValues();
            authValues.AuthType = CustomAuthenticationType.Custom;

            var auth0Data = new Dictionary<string, object>
            {
                { "type", "token" },
                { "format", "photon" },
                { "data", secretData }
            };

            authValues.SetAuthPostData(auth0Data);

            this.client.AuthValues = authValues;
            return true;
        }

        #region Photon IConnectionCallbacks
        void IConnectionCallbacks.OnConnected()
        {
            _logger.LogDebug("OnConnected");
        }

        void IConnectionCallbacks.OnDisconnected(DisconnectCause cause)
        {
            _logger.LogDebug($"OnDisconnected {cause}");
            connectionTCS?.TrySetResult(false);
        }

        void IConnectionCallbacks.OnConnectedToMaster()
        {
            var curRegion = TryGetRegion();
            _logger.LogWarning($"OnConnectedToMaster Server: {this.client.LoadBalancingPeer.ServerIpAddress} Region: {curRegion}");
            photonRoomListStorage.CurrentPhotonRegion = curRegion;

            connectionTCS?.TrySetResult(true);

            this.client.OpJoinLobby(new TypedLobby("default", LobbyType.Default));
        }

        #region Authentication
        void IConnectionCallbacks.OnCustomAuthenticationFailed(string debugMessage)
        {
            _logger.LogWarning($"OnCustomAuthenticationFailed {debugMessage}");
            connectionTCS?.TrySetResult(false);

            accountStorage.Clean();
        }

        void IConnectionCallbacks.OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            _logger.LogWarning($"OnCustomAuthenticationResponse");
            foreach(var kvp in data)
                _logger.LogWarning($"{kvp.Key}:{kvp.Value}");
        }
        #endregion

        void IConnectionCallbacks.OnRegionListReceived(RegionHandler regionHandler)
        {
            _logger.LogDebug("OnRegionListReceived");
        }
        #endregion

        #region Photon ILobbyCallbacks
        void ILobbyCallbacks.OnJoinedLobby()
        {
            _logger.LogDebug("OnJoinedLobby");
        }

        void ILobbyCallbacks.OnLeftLobby()
        {
            _logger.LogDebug("OnLeftLobby");
        }

        void ILobbyCallbacks.OnRoomListUpdate(List<RoomInfo> roomList)
        {
            _logger.LogDebug($"OnRoomListUpdate {roomList.Count}");
            photonRoomListStorage?.OnPhotonRoomListUpdated(roomList);

            if(photonConfig.ShowOnConsole)
                ShowOnConsole(true);
        }

        void ILobbyCallbacks.OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            _logger.LogDebug("OnLobbyStatisticsUpdate");
        }
        #endregion

        #region Getter
        bool ShouldTryConnectMaster()
        {
            if (this.client == null)
            {
                return false;
            }

            switch (this.client.State)
            {
                case ClientState.PeerCreated:
                case ClientState.Disconnected:
                    return true;
                default:
                    _logger.LogDebug(message: $"ShouldTryConnectMaster @{ this.client.State}");
                    break;
            }

            return false;
        }

        string TryGetRegion()
        {
            return this.client.CloudRegion.TrimEnd('/', '*');
        }
        #endregion

        #region 
        void ShowOnConsole(bool clearConsole = false)
        {
            try
            {

                var res = photonRoomListStorage.GetAllCachedRoom();
                //res.SetShouldSerializeCurrentTimestamp(true);

                res.SetShouldSerializeRegionStringInstead(true);

                if (clearConsole)
                    Console.Clear();

                _logger.LogInformation($"\n{DateTimeOffset.Now} [{DateTimeOffset.Now.ToUniversalTime()}]\n{res}");
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Exception: {e.ToString()} {DateTimeOffset.Now}");
            }
        }
        #endregion
    }
}