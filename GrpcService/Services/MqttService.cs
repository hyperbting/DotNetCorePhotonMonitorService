using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Text;
using MQTTnet.Client.Subscribing;

namespace GrpcService1
{
    public class MqttService : BackgroundService
    {
        private readonly ILogger<MqttService> _logger;
        private MQTTConfig mqttConfig;

        CancellationTokenSource createMQTTClientToken;

        public MqttService(ILogger<MqttService> logger, IConfiguration mqttserConfig)
        {
            _logger = logger;

            mqttConfig = new MQTTConfig();
            mqttserConfig.GetSection(MQTTConfig.MQTT).Bind(mqttConfig);

            _logger.LogInformation("MqttService Start @{time}", DateTimeOffset.Now);

            createMQTTClientToken = new CancellationTokenSource();
            _ = CreateMQTTClient(createMQTTClientToken.Token);
        }

        ~MqttService()
        {
            _logger.LogInformation("MqttService End @{time}", DateTimeOffset.Now);
        }

        //// https://github.com/chkr1011/MQTTnet/wiki/Client
        private static IMqttClient mqttClient = null;        
        private static IMqttClientOptions options = null;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("MqttService.ExecuteAsync Start@{time}", DateTimeOffset.Now);
            if (!mqttConfig.Publisher)
            {
                _logger.LogDebug("MqttService.ExecuteAsync NotPublisher @{time}", DateTimeOffset.Now);
                return;
            }

            await PublisherActor(stoppingToken);
            _logger.LogInformation("MqttService.ExecuteAsync End@{time}", DateTimeOffset.Now);
        }

        async Task CreateMQTTClient(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CreateMQTTClient Begin@{time}", DateTimeOffset.Now);
            // Create a new MQTT client.
            var factory = new MqttFactory();
            mqttClient = factory.CreateMqttClient();

            options = new MqttClientOptionsBuilder()
                .WithTcpServer(mqttConfig.Broker, mqttConfig.TcpPort)
                //.WithCredentials(UserId, Password)
                //.WithTls()
                //.WithCleanSession()
                .WithClientId(mqttConfig.ClientID)
                .Build();

            //// https://github.com/chkr1011/MQTTnet/wiki/Client#reconnecting
            mqttClient.UseDisconnectedHandler(async e =>
            {
                _logger.LogInformation("### DISCONNECTED FROM SERVER ###");
                await Task.Delay(TimeSpan.FromSeconds(5));

                try
                {
                    await mqttClient.ConnectAsync(options, CancellationToken.None); // Since 3.0.5 with CancellationToken
                }
                catch
                {
                    _logger.LogInformation("### RECONNECTING FAILED ###");
                }
            });

            if (!mqttConfig.Publisher)
            {
                _logger.LogInformation("CreateMQTTClient as Subscriber @{time}", DateTimeOffset.Now);
                await SubscriberActor();
            }
            else 
            {
                _logger.LogInformation("CreateMQTTClient as Publisher @{time}", DateTimeOffset.Now);
            }

            _logger.LogInformation("CreateMQTTClient ConnectAsync@{time}", DateTimeOffset.Now);
            await mqttClient.ConnectAsync(options, stoppingToken);
        }

        async Task SubscriberActor()
        {
            //while (mqttClient == null || !mqttClient.IsConnected)
            //{
            //    _logger.LogDebug("SubscriberActor Waiting@{time}", DateTimeOffset.Now);
            //    await Task.Delay(100);
            //}

            _logger.LogInformation("SubscriberActor Begin@{time}", DateTimeOffset.Now);

            //// https://github.com/chkr1011/MQTTnet/wiki/Client#consuming-messages
            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                var pls = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                var pl = pls.Split('|');
                var curr = DateTimeOffset.Now;
                var delta = long.Parse(pl[0]);
                delta = curr.ToUnixTimeMilliseconds() - delta;

                var serialNumber = -1;
                if (pl.Length>=2)
                    serialNumber = int.Parse(pl[1]);

                _logger.LogInformation("### RECEIVED APPLICATION MESSAGE ### {serial} Delta:{delta} @{time} ", serialNumber, delta, curr);
                _logger.LogDebug($"Topic:{e.ApplicationMessage.Topic}" +
                    $", Payload:{Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}" + 
                    $", QoS:{e.ApplicationMessage.QualityOfServiceLevel}" + 
                    $", Retain:{e.ApplicationMessage.Retain}");

                //// Task.Run(() => mqttClient.PublishAsync("hello/world/T1v10"));
            });

            //// https://github.com/chkr1011/MQTTnet/wiki/Client#subscribing-to-a-topic
            mqttClient.UseConnectedHandler(async e =>
            {
                _logger.LogInformation("### CONNECTED WITH SERVER ### @{time}", DateTimeOffset.Now);

                // Subscribe to a topic
                await mqttClient.SubscribeAsync(new MqttClientSubscribeOptionsBuilder()
                    .WithTopicFilter("hello/world/T1v10")
                    .Build()
                );

                _logger.LogInformation("### SUBSCRIBED ###");
            });

            _logger.LogInformation("SubscriberActor End@{time}", DateTimeOffset.Now);
        }

        int maxCounter = 10000;
        long beginTimeMS = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        async Task PublisherActor(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PublisherActor Begin@{time}", DateTimeOffset.Now);

            int counter = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                if (mqttClient == null || !mqttClient.IsConnected)
                {
                    _logger.LogDebug("PublisherActor Waiting@{time}", DateTimeOffset.Now);
                    await Task.Delay(1000);
                    continue;
                }
                string payload = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() + "|"+ counter.ToString();
                var fakeData = buildFakeData(128, payload);
                var message = new MqttApplicationMessageBuilder()
                .WithTopic("hello/world/T1v10")
                .WithPayload(fakeData)
                //.WithExactlyOnceQoS()
                //.WithRetainFlag()
                .Build();

                _logger.LogDebug("PublisherActor Sending {cnt} {msg} @{time}", counter, message.ConvertPayloadToString(), DateTimeOffset.Now);
                var res = await mqttClient.PublishAsync(message, stoppingToken); // Since 3.0.5 with CancellationToken
                if(!String.IsNullOrWhiteSpace(res.ReasonString))
                    _logger.LogWarning("PublisherActor {code} {res} @{time}", res.ReasonCode, res.ReasonString, DateTimeOffset.Now);

                if (counter >= maxCounter)
                {
                    var sendingPeriod = DateTimeOffset.Now.ToUnixTimeMilliseconds() - beginTimeMS;
                    _logger.LogWarning("PublisherActor Finished sending {mCoun} in {period}ms @{time}", maxCounter, sendingPeriod, DateTimeOffset.Now);
                    counter = 0;

                    await Task.Delay(10000);

                    beginTimeMS = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                }

                counter++;
                if (mqttConfig.PublisherSendDelay>0)
                    await Task.Delay(mqttConfig.PublisherSendDelay);
            }
            _logger.LogDebug("PublisherActor End@{time}", DateTimeOffset.Now);
        }

        string buildFakeData(int tarLen, string str="")
        {
            str += "|";
            while (str.Length < tarLen)
                str += Guid.NewGuid().ToString();

            // shrink to tarLen
            str = str.Substring(0, tarLen);
            return str;
        }
    }
}