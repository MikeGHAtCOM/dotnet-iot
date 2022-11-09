using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBlade.API.dotnet.client.core.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Config
    {
        public DateTime cloudUpdateTime { get; set; }
        public string version { get; set; }

        public Config()
        {
            version = String.Empty;
        }
    }

    public class Credential
    {
        public string expirationTime { get; set; }
        public PublicKey publicKey { get; set; }

        public Credential()
        {
            expirationTime = String.Empty;
            publicKey = new PublicKey();
        }
    }

    public class DeviceCollection
    {
        public List<DeviceModel> devices { get; set; }

        public DeviceCollection()
        {
            devices = new List<DeviceModel>();
        }
    }

    /// <summary>
    /// Class that represents a device
    /// </summary>
    public class DeviceModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string numId { get; set; }
        public List<Credential> credentials { get; set; }
        public string lastHeartbeatTime { get; set; }
        public string lastEventTime { get; set; }
        public string lastStateTime { get; set; }
        public string lastConfigAckTime { get; set; }
        public string lastConfigSendTime { get; set; }
        public bool blocked { get; set; }
        public string lastErrorTime { get; set; }
        public LastErrorStatus lastErrorStatus { get; set; }
        public Config config { get; set; }
        public State state { get; set; }
        public string logLevel { get; set; }
        public Metadata metadata { get; set; }
        public GatewayConfig gatewayConfig { get; set; }

        public DeviceModel()
        {
            id = string.Empty;
            name = string.Empty;
            numId = string.Empty;
            credentials = new List<Credential>();
            lastHeartbeatTime = string.Empty;
            lastEventTime = string.Empty;
            lastStateTime = string.Empty;
            lastConfigAckTime = string.Empty;
            lastConfigSendTime = string.Empty;
            lastErrorTime = string.Empty;
            lastErrorStatus = new LastErrorStatus();
            config = new Config();
            state = new State();
            logLevel = "NONE";
            metadata = new Metadata();
            gatewayConfig = new GatewayConfig();
        }
    }

    public class GatewayConfig
    {
        public string lastAccessedGatewayId { get; set; }
        public string lastAccessedGatewayTime { get; set; }

        public GatewayConfig()
        {
            lastAccessedGatewayId = string.Empty;
            lastAccessedGatewayTime = string.Empty;
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
