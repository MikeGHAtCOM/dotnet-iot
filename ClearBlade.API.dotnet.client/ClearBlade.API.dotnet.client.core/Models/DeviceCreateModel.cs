using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBlade.API.dotnet.client.core.Models
{
    public class DeviceCreateModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public int numId { get; set; }
        public List<object> credentials { get; set; }
        public LastErrorStatus lastErrorStatus { get; set; }
        public Config config { get; set; }
        public State state { get; set; }
        public string logLevel { get; set; }
        public Metadata metadata { get; set; }
        public GatewayConfig gatewayConfig { get; set; }

        public DeviceCreateModel()
        {
            id = String.Empty;
            name = String.Empty;
            numId = 0;
            credentials = new List<object>();
            lastErrorStatus = new LastErrorStatus();
            config = new Config();
            state = new State();
            logLevel = String.Empty;
            metadata = new Metadata();
            gatewayConfig = new GatewayConfig();
        }
    }
}
