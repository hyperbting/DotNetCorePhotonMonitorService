using Photon.Realtime;

using PhotonRoomListGrpcService.Interfaces.IStorages;

using System;
using System.Collections.Generic;

namespace PhotonRoomListGrpcService.Models.Storages
{
    public class PhotonRooms : IRoomList
    {
        private readonly Dictionary<string, RoomInfo> cachedRoomList;

        public PhotonRooms()
        {
            cachedRoomList = new Dictionary<string, RoomInfo>();
            OnPhotonRoomListUpdated += UpdateCachedRoomList;
        }

        ~PhotonRooms()
        {
            OnPhotonRoomListUpdated -= UpdateCachedRoomList;
            cachedRoomList.Clear();
        }

        #region IRoomList
        public Action<List<RoomInfo>> OnPhotonRoomListUpdated { get; set; }
        public RegionInGameUserCount GetAllCachedRoom()
        {

            var res = new RegionInGameUserCount();

            List<InGameUserCount> iguc = new();
            foreach (var kvp in cachedRoomList)
            {
                var info = kvp.Value;
                iguc.Add(new InGameUserCount()
                {
                    roomName = info.Name,
                    count = info.PlayerCount
                });
            }

            res.uCounts = iguc.ToArray();

            return res;
        }
        #endregion

        void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                RoomInfo info = roomList[i];

                if (info.RemovedFromList)
                {
                    cachedRoomList.Remove(info.Name);
                }
                else
                {
                    cachedRoomList[info.Name] = info;
                }
            }
        }

        [Serializable]
        public class RegionInGameUserCount
        {
            public string region;
            public InGameUserCount[] uCounts;
        }

        [Serializable]
        public class InGameUserCount
        {
            public string roomName;
            public int count;
        }
    }
}