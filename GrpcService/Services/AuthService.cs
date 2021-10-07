using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcService1
{
    public class AuthService: BackgroundService
    {
        public static OauthResponse oauthResp;
        public static bool TryGetAuthInfo(out string jwtstring)
        {
            jwtstring = "";

            if (oauthResp == null)
                return false;

            jwtstring = oauthResp.access_token;
            return true;
        }

        private readonly ILogger<AuthService> _logger;
        AuthConfig authConfig;
        public AuthService(ILogger<AuthService> logger , IConfiguration aConfig )
        {
            _logger = logger;

            authConfig = new AuthConfig();
            aConfig.GetSection(AuthConfig.Auth).Bind(authConfig);
            _logger.LogDebug(JsonConvert.SerializeObject(authConfig));

        }

        ~AuthService()
        {
            hclient.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                try
                {
                    if (AuthService.oauthResp == null)
                        await PostAuth();

                    await Task.Delay(1000, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        private static readonly HttpClient hclient = new HttpClient();

        public async Task PostAuth(AuthConfig authConfig = null)//(string target, IEnumerable<KeyValuePair<string?, string?>> values)
        {
            if (authConfig == null)
                authConfig = this.authConfig;

            if (authConfig == null || string.IsNullOrEmpty(authConfig.OauthAddress))
            {
                _logger.LogError("No OauthAddress");
                return;
            }

            var response = await hclient.PostAsync(authConfig.OauthAddress, new FormUrlEncodedContent(authConfig.BuildRequest()));

            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseString);

            AuthService.oauthResp = JsonConvert.DeserializeObject<OauthResponse>(responseString);

            Console.WriteLine(oauthResp);
        }
    }

}