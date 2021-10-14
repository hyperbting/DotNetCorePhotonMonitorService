using System.Collections.Generic;

[System.Serializable]
public class MQTTConfig
{
    public const string MQTT = "MQTT";
    public string Broker { get; set; }

    public int TcpPort { get; set; }

    public string ClientID { get; set; }

    public bool Publisher { get; set; }
    public int PublisherSendDelay { get; set; }
}