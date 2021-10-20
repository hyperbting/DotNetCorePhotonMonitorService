using Photon.Realtime;

using PhotonRoomListGrpcService.Models.Storages;

using System;
using System.Collections.Generic;

namespace PhotonRoomListGrpcService.Interfaces.IStorages
{
    public interface IRoomList
    {
        public Action<List<RoomInfo>> OnPhotonRoomListUpdated { get; set; }
        public PhotonRooms.RegionInGameUserCount GetAllCachedRoom();
    }
}