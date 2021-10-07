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

            photonConfig = new PhotonConfig();
            phoConfig.GetSection(PhotonConfig.Photon).Bind(photonConfig);

            this.client.AddCallbackTarget(this);
            this.client.StateChanged += this.OnStateChange;

            Console.WriteLine($"PhotonUser. {JsonConvert.SerializeObject(phoConfig)}");

            this.client.AppId = photonConfig.AppId;

            //_ = TryConnectToMasterServer(photonConfig.Region[0]);

            //// for Photon
            //Thread t = new Thread(this.Loop);
            //t.Start();
        }

        ~PhotonService()
        {
            this.client.Disconnect();
            this.client.RemoveCallbackTarget(this);
        }

        private void OnStateChange(ClientState arg1, ClientState arg2)
        {
            Console.WriteLine(arg1 + " -> " + arg2);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                this.client.Service();
                Thread.Sleep(100);
                //_logger.LogInformation("{time} ", DateTimeOffset.Now);
                try
                {
                    await TryConnectToMasterServer(photonConfig.Region[0]);
                    //await Task.Delay(10000, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        private void Loop(object state)
        {
            while (true)
            {
                this.client.Service();
                Thread.Sleep(33);
            }
        }

        #region IConnectionCallbacks
        void IConnectionCallbacks.OnConnected()
        {
            Console.WriteLine("OnConnected");
        }

        void IConnectionCallbacks.OnDisconnected(DisconnectCause cause)
        {
            Console.WriteLine($"OnDisconnected {cause}");
        }

        void IConnectionCallbacks.OnConnectedToMaster()
        {
            Console.WriteLine($"OnConnectedToMaster Server: {this.client.LoadBalancingPeer.ServerIpAddress} Region: {this.client.CloudRegion}");
            this.client.OpJoinLobby(new TypedLobby("default", LobbyType.Default));
        }

        #region Authentication
        void IConnectionCallbacks.OnCustomAuthenticationFailed(string debugMessage)
        {
            throw new NotImplementedException();
        }

        void IConnectionCallbacks.OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            throw new NotImplementedException();
        }
        #endregion

        void IConnectionCallbacks.OnRegionListReceived(RegionHandler regionHandler)
        {
            Console.WriteLine("OnRegionListReceived");
        }
        #endregion

        TaskCompletionSource connectionTCS;
        public async Task TryConnectToMasterServer(string targetRegion)
        {
            _logger.LogDebug("TryConnectToMasterServer");

            var jwtString = "";
            if (AuthService.TryGetAuthInfo(out jwtString))
            {
                TrySetAuthenticationValues();
                connectionTCS = new TaskCompletionSource();

                this.client.ConnectToRegionMaster(targetRegion);

                //wait until connected
                while (connectionTCS != null && !connectionTCS.Task.IsCompleted)
                {
                    await Task.Delay(100);
                }
            }
        }

        bool TrySetAuthenticationValues(string userID, string password)
        {
            AuthenticationValues authValues = new AuthenticationValues();
            authValues.AuthType = CustomAuthenticationType.Custom;
            //authValues.AddAuthParameter("user", userID);
            //authValues.AddAuthParameter("pass", password);

            authValues.AddAuthParameter("type", "token");
            authValues.AddAuthParameter("format", "photon");
            authValues.AddAuthParameter("data", password);
            authValues.UserId = userID; // this is required when you set UserId directly from client and not from web service

            this.client.AuthValues = authValues;

            return true;
        }

        //bool CanConnect()
        //{
        //    switch (this.client.State)
        //    {
        //        case ClientState.Disconnected:
        //        case ClientState.PeerCreated:
        //            return true;

        //    }

        //    return false;
        //}

        #region ILobbyCallbacks
        public void OnJoinedLobby()
        {
            Console.WriteLine("OnJoinedLobby");
        }

        public void OnLeftLobby()
        {
            Console.WriteLine("OnLeftLobby");
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Console.WriteLine($"OnRoomListUpdate {roomList.Count}");
            UpdateCachedRoomList(roomList);
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            Console.WriteLine("OnLobbyStatisticsUpdate");
        }
        #endregion

        private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();
        public RegionInGameUserCount GetAllCachedRoom()
        {
            var res = new RegionInGameUserCount() { 
                region=this.client.CloudRegion.TrimEnd('/','*'),
            };

            //Console.WriteLine($"GetAllCachedRoom Begin");
            //var sb = new StringBuilder();

            List<InGameUserCount> iguc = new List<InGameUserCount>();
            foreach (var kvp in cachedRoomList)
            {
                var info = kvp.Value;
                //sb.Append($" {info.Name}:{info.PlayerCount}");
                iguc.Add(new InGameUserCount() {
                    roomName=info.Name,
                    count=info.PlayerCount
                });
            }
            res.uCounts = iguc.ToArray();

            //if (sb.Length > 0)
            //    Console.WriteLine(sb.ToString());
            //Console.WriteLine($"GetAllCachedRoom End");

            return res;
        }

        private void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                RoomInfo info = roomList[i];
                //Console.WriteLine($"UpdateCachedRoomList {i}:" + info.ToString());

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

            //var resString = JsonConvert.SerializeObject(res);
            //Console.WriteLine(resString);
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