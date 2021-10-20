using Photon.Realtime;
using PhotonRoomListGrpcService.Interfaces.IStorages;
using System;
using System.Collections.Generic;

namespace PhotonRoomListGrpcService.Models.Storages
{
    public class PhotonRooms : IRoomList
    {
        private Dictionary<string, RoomInfo> cachedRoomList;

        private string assignedRegion = "";

        public PhotonRooms()
        {
            cachedRoomList = new Dictionary<string, RoomInfo>();
        }

        ~PhotonRooms()
        {
            cachedRoomList.Clear();
        }

        public RegionInGameUserCount GetAllCachedRoom(string tarRegion = "")
        {
            if (!string.IsNullOrWhiteSpace(tarRegion))
            {
                assignedRegion = tarRegion;
            }

            var res = new RegionInGameUserCount()
            {
                region = assignedRegion,
            };

            List<InGameUserCount> iguc = new List<InGameUserCount>();
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

        public void UpdateCachedRoomList(List<RoomInfo> roomList)
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