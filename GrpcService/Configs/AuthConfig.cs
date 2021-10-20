using System.Collections.Generic;

namespace PhotonRoomListGrpcService.Configs
{
    [System.Serializable]
    public class AuthConfig
    {
        public const string Auth = "Auth";
        public string OauthAddress { get; set; }

        public string grant_type { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string audience { get; set; }
        public string scope { get; set; }

        public string username { get; set; }
        public string password { get; set; }

        public IEnumerable<KeyValuePair<string, string>> BuildRequest()
        {
            return new Dictionary<string, string>
            {
                { "grant_type", grant_type },
                { "client_id", client_id },
                { "client_secret", client_secret },
                { "audience", audience },
                { "scope", scope},

                { "username", username },
                { "password", password }
            };
        }
    }
}