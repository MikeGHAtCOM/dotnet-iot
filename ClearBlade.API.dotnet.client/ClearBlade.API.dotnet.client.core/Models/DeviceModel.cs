namespace ClearBlade.API.dotnet.client.core.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Config
    {
        public DateTime CloudUpdateTime { get; set; }
        public string Version { get; set; }

        public Config()
        {
            Version = String.Empty;
        }
    }

    public class DeviceCollection
    {
        public List<DeviceModel> Devices { get; set; }

        public DeviceCollection()
        {
            Devices = new List<DeviceModel>();
        }
    }

    public class DeviceCredential
    {
        public object? ExpirationTime { get; set; }
        public PublicKeyCredential PublicKey { get; set; }
        public string ETag { get; set; }

        public DeviceCredential()
        {
            PublicKey = new PublicKeyCredential();
            ETag = string.Empty;
        }
        public DeviceCredential(object expirationTime, PublicKeyCredential publicKey, string eTag)
        {
            ExpirationTime = expirationTime;
            PublicKey = publicKey;
            ETag = eTag;
        }
    }

    public class PublicKeyCredential
    {
        public string Format { get; set; }
        public string Key { get; set; }
        public string ETag { get; set; }

        public PublicKeyCredential()
        {
            Format = String.Empty;
            Key = String.Empty;
            ETag = String.Empty;
        }
        public PublicKeyCredential(string format, string key, string eTag)
        {
            Format = format;
            Key = key;
            ETag = eTag;
        }
    }

    /// <summary>
    /// Class that represents a device
    /// </summary>
    public class DeviceModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NumId { get; set; }
        public List<DeviceCredential> Credentials { get; set; }
        public string LastHeartbeatTime { get; set; }
        public string LastEventTime { get; set; }
        public string LastStateTime { get; set; }
        public string LastConfigAckTime { get; set; }
        public string LastConfigSendTime { get; set; }
        public bool Blocked { get; set; }
        public string LastErrorTime { get; set; }
        public LastErrorStatus LastErrorStatus { get; set; }
        public Config Config { get; set; }
        public State State { get; set; }
        public string LogLevel { get; set; }
        public Metadata Metadata { get; set; }
        public GatewayConfig GatewayConfig { get; set; }

        public DeviceModel()
        {
            Id = string.Empty;
            Name = string.Empty;
            NumId = string.Empty;
            Credentials = new List<DeviceCredential>();
            LastHeartbeatTime = string.Empty;
            LastEventTime = string.Empty;
            LastStateTime = string.Empty;
            LastConfigAckTime = string.Empty;
            LastConfigSendTime = string.Empty;
            LastErrorTime = string.Empty;
            LastErrorStatus = new LastErrorStatus();
            Config = new Config();
            State = new State();
            LogLevel = "NONE";
            Metadata = new Metadata();
            GatewayConfig = new GatewayConfig();
        }
    }

    public class GatewayConfig
    {
        public string LastAccessedGatewayId { get; set; }
        public string LastAccessedGatewayTime { get; set; }
        public string GatewayType { get; set; }
        public string GatewayAuthMethod { get; set; }

        public GatewayConfig()
        {
            LastAccessedGatewayId = string.Empty;
            LastAccessedGatewayTime = string.Empty;
            GatewayType = string.Empty;
            GatewayAuthMethod = string.Empty;
        }
    }

    public class LastErrorStatus
    {
        public int code { get; set; }
        public string message { get; set; }

        public LastErrorStatus()
        {
            message = string.Empty;
        }
    }

    public class Metadata
    {
    }

    public class PublicKey
    {
        public string format { get; set; }
        public string key { get; set; }

        public PublicKey()
        {
            format = string.Empty;
            key = string.Empty;
        }
    }

    public class State
    {
        public string updateTime { get; set; }
        public string binaryData { get; set; }

        public State()
        {
            updateTime = String.Empty;
            binaryData = String.Empty;
        }
    }


}
