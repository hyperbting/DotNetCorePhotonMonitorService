using PhotonRoomListGrpcService.Interfaces.Storages;

namespace PhotonRoomListGrpcService.Models.Storages
{
    public class OauthAccount : IAccountStorage
    {
        private OauthResponse oauthResp;

        public OauthAccount()
        {
        }

        ~OauthAccount()
        {
        }

        #region IAccountStorage
        public bool IsValid
        {
            get
            {
                if (oauthResp == null)
                    return false;

                return true;
            }
        }

        public void Store(OauthResponse response)
        {
            oauthResp = response;
        }

        public void Clean()
        {
            oauthResp = null;
        }

        public bool TryGetAuthInfo(out string jwtstring)
        {
            jwtstring = "";

            if (oauthResp == null)
                return false;

            jwtstring = oauthResp.access_token;
            return true;
        }
        #endregion
    }
}