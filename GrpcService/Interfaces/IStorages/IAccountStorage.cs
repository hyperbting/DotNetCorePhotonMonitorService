using PhotonRoomListGrpcService.Models;

public interface IAccountStorage
{
    bool IsValid{ get; }

    void Store(OauthResponse response);
    void Clean();
    bool TryGetAuthInfo(out string jwtstring);

}