using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Photon.Realtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GrpcService1
{
    public class PhotonService : BackgroundService, IConnectionCallbacks, ILobbyCallbacks//, IInRoomCallbacks, IMatchmakingCallbacks
    {
        private readonly ILogger<PhotonService> _logger;
        private readonly PhotonConfig photonConfig;

        private readonly LoadBalancingClient client = new LoadBalancingClient();

        public PhotonService(ILogger<PhotonService> logger, IConfiguration phoConfig)
        {
            _logger = logger;

            _logger.LogInformation("PhotonService Start @{time}", DateTimeOffset.Now);

            photonConfig = new PhotonConfig();
            phoConfig.GetSection(PhotonConfig.Photon).Bind(photonConfig);

            this.client.AddCallbackTarget(this);
            this.client.StateChanged += this.OnStateChange;

            //_logger.LogDebug($"PhotonUser. {JsonConvert.SerializeObject(phoConfig)}");

            this.client.AppId = photonConfig.AppId;

            //_ = TryConnectToMasterServer(photonConfig.Region[0]);
        }

        ~PhotonService()
        {
            _logger.LogInformation("PhotonService End @{time}", DateTimeOffset.Now);

            this.client.Disconnect();
            this.client.RemoveCallbackTarget(this);
        }

        private void OnStateChange(ClientState arg1, ClientState arg2)
        {
            _logger.LogInformation($"PhotonStateChanges: {arg1} -> { arg2}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PhotonService ExecuteAsync@{time} ", DateTimeOffset.Now);
            _ = AsyncLoop(stoppingToken);

            _ = TryConnectToMasterServer(photonConfig.Region[0], stoppingToken);            
        }

        async Task AsyncLoop(CancellationToken stoppingToken)
        {
            _logger.LogDebug("PhotonService AsyncLoop Start@{time} ", DateTimeOffset.Now);
            while (!stoppingToken.IsCancellationRequested)
            {
                this.client.Service();
                await Task.Delay(33);
            }
            _logger.LogDebug("PhotonService AsyncLoop End@{time} ", DateTimeOffset.Now);
        }

        #region IConnectionCallbacks
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
            _logger.LogDebug($"OnConnectedToMaster Server: {this.client.LoadBalancingPeer.ServerIpAddress} Region: {this.client.CloudRegion}");

            connectionTCS?.TrySetResult(true);

            this.client.OpJoinLobby(new TypedLobby("default", LobbyType.Default));
        }

        #region Authentication
        void IConnectionCallbacks.OnCustomAuthenticationFailed(string debugMessage)
        {
            _logger.LogWarning($"OnCustomAuthenticationFailed {debugMessage}");
            connectionTCS?.TrySetResult(false);

            AuthService.CleanOauthResponse();
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

        TaskCompletionSource<bool> connectionTCS;
        public async Task TryConnectToMasterServer(string targetRegion, CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (this.client.IsConnectedAndReady)
                {
                    _logger.LogDebug($"TryConnectToMasterServer AlreadyInServer {this.client.State}");
                    await Task.Delay(1000);
                    continue;
                }

                var jwtString = "";
                if (!AuthService.TryGetAuthInfo(out jwtString))
                {
                    _logger.LogDebug($"TryConnectToMasterServer NoAuthFound {this.client.State}");
                    await Task.Delay(1000);
                    continue;
                }

                _logger.LogInformation("PhotonService.TryConnectToMasterServer @{time} ", DateTimeOffset.Now);

                _logger.LogDebug("TryConnectToMasterServer.TrySetAuthenticationValues");
                TrySetAuthenticationValues(jwtString);

                connectionTCS = new TaskCompletionSource<bool>();
                this.client.ConnectToRegionMaster(targetRegion);

                var cts = new CancellationTokenSource();
                cts.CancelAfter(10000);

                _logger.LogDebug("TryConnectToMasterServer Wait for ConnectToRegionMaster");
                //wait until connected or cancelled
                while (connectionTCS != null && !connectionTCS.Task.IsCompleted && !cts.IsCancellationRequested)
                {
                    try
                    {
                        await Task.Delay(100);
                    }
                    catch (TaskCanceledException)
                    {
                        _logger.LogWarning("TryConnectToMasterServer Cancelled");
                        connectionTCS.TrySetResult(false);

                        return;
                    }
                }

                await Task.Delay(3000);
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

        #region ILobbyCallbacks
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
            UpdateCachedRoomList(roomList);
        }

        void ILobbyCallbacks.OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            _logger.LogDebug("OnLobbyStatisticsUpdate");
        }
        #endregion

        private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();
        public RegionInGameUserCount GetAllCachedRoom()
        {
            var res = new RegionInGameUserCount() { 
                region=this.client.CloudRegion.TrimEnd('/','*'),
            };

            //_logger.LogDebug($"GetAllCachedRoom Begin");

            List<InGameUserCount> iguc = new List<InGameUserCount>();
            foreach (var kvp in cachedRoomList)
            {
                var info = kvp.Value;
                iguc.Add(new InGameUserCount() {
                    roomName=info.Name,
                    count=info.PlayerCount
                });
            }

            res.uCounts = iguc.ToArray();

            return res;
        }

        private void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                RoomInfo info = roomList[i];
                _logger.LogDebug($"UpdateCachedRoomList {i} mc:{info.masterClientId}:" + info.ToStringFull());

                if (info.RemovedFromList)
                {
                    cachedRoomList.Remove(info.Name);
                }
                else
                {
                    cachedRoomList[info.Name] = info;
                }
            }

            var res = GetAllCachedRoom();

            var resString = JsonConvert.SerializeObject(res, Formatting.Indented);
            //Console.Clear();
            _logger.LogInformation($"{DateTimeOffset.Now} \n {resString}");
            
        }

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
                    _logger.LogDebug($"ShouldTryConnectMaster @{ this.client.State.ToString()}");
                    break;
            }

            return false;
        }
    }

    [System.Serializable]
    public class RegionInGameUserCount
    {
        public string region;
        public InGameUserCount[] uCounts;
    }

    [System.Serializable]
    public class InGameUserCount
    {
        public string roomName;
        public int count;
    }

}