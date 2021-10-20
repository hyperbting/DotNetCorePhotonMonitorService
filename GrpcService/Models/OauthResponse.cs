using Newtonsoft.Json;

namespace PhotonRoomListGrpcService.Models
{
    [System.Serializable]
    public class OauthResponse
    {
        public static OauthResponse BuildOauthResponse(string txt)
        {
            OauthResponse res = JsonConvert.DeserializeObject<OauthResponse>(txt);

            return res;
        }

        public string access_token;
        public string id_token;
        public string scope;
        public string token_type;
        public int expires_in;

        public string error;
        public string error_description;

        public bool IsValid()
        {
            if (!string.IsNullOrEmpty(error) || !string.IsNullOrEmpty(error_description))
                return false;

            return true;
        }
    }
}