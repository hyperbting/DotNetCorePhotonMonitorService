using Newtonsoft.Json;

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

    public void TryParseJWT()
    {
        if (string.IsNullOrEmpty(access_token))
            return;


    }
}