using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBlade.API.dotnet.client.core.Models
{
    public class DeviceCreateResultModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string numId { get; set; }
        public List<object> credentials { get; set; }
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

        public DeviceCreateResultModel()
        {
            id = String.Empty;
            name = String.Empty;
            numId = String.Empty;
            credentials = new List<object>();
            lastHeartbeatTime = String.Empty;
            lastEventTime = String.Empty;
            lastStateTime = String.Empty;
            lastConfigAckTime = String.Empty;
            lastConfigSendTime = String.Empty;
            lastErrorTime = String.Empty;
            lastErrorStatus = new LastErrorStatus();
            config = new Config();
            state = new State();
            logLevel = String.Empty;
            metadata = new Metadata();
            gatewayConfig = new GatewayConfig();
        }
    }
}
