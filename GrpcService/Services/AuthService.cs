using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PhotonRoomListGrpcService.Configs;
using PhotonRoomListGrpcService.Interfaces.Storages;
using PhotonRoomListGrpcService.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhotonRoomListGrpcService
{
    public class AuthService: BackgroundService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly AuthConfig authConfig;

        private readonly IAccountStorage accountStorage;

        public AuthService(ILogger<AuthService> logger , IConfiguration aConfig, IAccountStorage accStore)
        {
            _logger = logger;

            accountStorage = accStore;

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
                    if (!accountStorage.IsValid)
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

            var response = await hclient.PostAsync(
                authConfig.OauthAddress, 
                new FormUrlEncodedContent(authConfig.BuildRequest())
            );

            var responseString = await response.Content.ReadAsStringAsync();

            _logger.LogDebug(responseString);

            accountStorage.Store(JsonConvert.DeserializeObject<OauthResponse>(responseString));
            if (!accountStorage.IsValid)
            { 
                _logger.LogWarning($"AuthService.PostAuth ResultNotValid");
                accountStorage.Clean();
                await Task.Delay(5000);

                return;
            }

            _logger.LogDebug($"{responseString}");
        }
    }
}