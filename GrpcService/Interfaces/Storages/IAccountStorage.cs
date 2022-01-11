using PhotonRoomListGrpcService.Models;

namespace PhotonRoomListGrpcService.Interfaces.Storages
{
    public interface IAccountStorage
    {
        bool IsValid { get; }

        void Store(OauthResponse response);
        void Clean();
        bool TryGetAuthInfo(out string jwtstring);

    }
}