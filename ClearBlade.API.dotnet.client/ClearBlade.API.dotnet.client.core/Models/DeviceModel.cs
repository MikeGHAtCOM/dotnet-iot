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
    }

    public class Credential
    {
        public string expirationTime { get; set; }
        public PublicKey publicKey { get; set; }
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
        public DateTime lastHeartbeatTime { get; set; }
        public DateTime lastEventTime { get; set; }
        public string lastStateTime { get; set; }
        public DateTime lastConfigAckTime { get; set; }
        public DateTime lastConfigSendTime { get; set; }
        public bool blocked { get; set; }
        public DateTime lastErrorTime { get; set; }
        public LastErrorStatus lastErrorStatus { get; set; }
        public Config config { get; set; }
        public State state { get; set; }
        public string logLevel { get; set; }
        public Metadata metadata { get; set; }
        public GatewayConfig gatewayConfig { get; set; }
    }

    public class GatewayConfig
    {
        public string lastAccessedGatewayId { get; set; }
        public string lastAccessedGatewayTime { get; set; }
    }

    public class LastErrorStatus
    {
        public int code { get; set; }
        public string message { get; set; }
    }

    public class Metadata
    {
    }

    public class PublicKey
    {
        public string format { get; set; }
        public string key { get; set; }
    }

    public class State
    {
        public string updateTime { get; set; }
        public string binaryData { get; set; }
    }


}
