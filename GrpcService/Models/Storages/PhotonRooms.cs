using Photon.Realtime;

using PhotonRoomListGrpcService.Interfaces.IStorages;

using System;
using System.Collections.Generic;

namespace PhotonRoomListGrpcService.Models.Storages
{
    public class PhotonRooms : IRoomList
    {        
        private readonly Dictionary<string, RoomInfo> cachedRoomList;

        private PhotonRegion targetPhotonRegion;
        private PhotonRegion photonRegion;

        public PhotonRooms()
        {
            targetPhotonRegion = PhotonRegion.Unknown;
            photonRegion = PhotonRegion.Unknown;
            
            cachedRoomList = new Dictionary<string, RoomInfo>();
            OnPhotonRoomListUpdated += UpdateCachedRoomList;
        }

        ~PhotonRooms()
        {
            OnPhotonRoomListUpdated -= UpdateCachedRoomList;
            cachedRoomList.Clear();
        }

        #region IRoomList
        public Action<string, string> OnTargetPhotonRegionChanged { get; set; }
        public DateTimeOffset LastUpdated { get; set; } = DateTimeOffset.UtcNow;

        #region Target/ Current Region
        public string TargetPhotonRegion
        {
            get 
            {
                return targetPhotonRegion.ToString();
            }
            set 
            {
                Enum.TryParse(value, true, out targetPhotonRegion);

                if (!IsRegionMatching())
                {
                    OnTargetPhotonRegionChanged?.Invoke(photonRegion.ToString(), targetPhotonRegion.ToString());
                }
            }
        }

        public string CurrentPhotonRegion
        {
            get
            {
                return photonRegion.ToString();
            }
            set
            {
                Enum.TryParse(value, true, out photonRegion);
            }
        }

        public bool IsRegionMatching()
        {
            if (targetPhotonRegion != photonRegion)
            {
                return false;
            }

            return true;
        }
        #endregion

        public Action<List<RoomInfo>> OnPhotonRoomListUpdated { get; set; }
        public RegionInGameUserCount GetAllCachedRoom()
        {
            RegionInGameUserCount regInGameuCount = new()
            {
                Region = photonRegion,
            };

            regInGameuCount.SetLastUpdate(LastUpdated);

            List<InGameUserCount> iguc = new();
            foreach (var kvp in cachedRoomList)
            {
                iguc.Add(kvp.Value.ToInGameUserCount());
            }

            regInGameuCount.uCounts = iguc.ToArray();

            return regInGameuCount;
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

            LastUpdated = DateTimeOffset.Now;
        }
    }

    public static class RoomInfoExtension
    {
        public static InGameUserCount ToInGameUserCount(this RoomInfo pri)
        {
            return new InGameUserCount()
            {
                RoomName = pri.Name,
                Count = pri.PlayerCount
            };
        }
    }
}