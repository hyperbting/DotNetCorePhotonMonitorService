using Photon.Realtime;
using PhotonRoomListGrpcService.Models.Storages;
using System.Collections.Generic;

namespace PhotonRoomListGrpcService.Interfaces.IStorages
{
    public interface IRoomList
    {
        public PhotonRooms.RegionInGameUserCount GetAllCachedRoom(string tarRegion = "");
        public void UpdateCachedRoomList(List<RoomInfo> roomList);
    }
}