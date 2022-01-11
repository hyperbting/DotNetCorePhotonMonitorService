using Photon.Realtime;

using System;
using System.Collections.Generic;

namespace PhotonRoomListGrpcService.Interfaces.Storages
{
    public interface IRoomList
    {
        #region Region
        public Action<string, string> OnTargetPhotonRegionChanged { get; set; }
        public Action<string, string> OnPhotonRegionChanged { get; set; }
        public string TargetPhotonRegion { get; set; }
        public string CurrentPhotonRegion { get; set; }
        public bool IsRegionMatching();
        #endregion

        public Action<List<RoomInfo>> OnPhotonRoomListUpdated { get; set; }
        public RegionInGameUserCount GetAllCachedRoom();
        public DateTimeOffset LastUpdated { get; set; }
    }
}