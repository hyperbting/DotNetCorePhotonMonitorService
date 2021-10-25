using Photon.Realtime;

using System;
using System.Collections.Generic;

namespace PhotonRoomListGrpcService.Interfaces.IStorages
{
    public interface IRoomList
    {
        public Action<string,string> OnTargetPhotonRegionChanged { get; set; }
        public string TargetPhotonRegion { get; set; }
        public string CurrentPhotonRegion { get; set; }
        public bool IsRegionMatching();

        public Action<List<RoomInfo>> OnPhotonRoomListUpdated { get; set; }
        public RegionInGameUserCount GetAllCachedRoom();
        public DateTimeOffset LastUpdated { get; set; }
    }
}