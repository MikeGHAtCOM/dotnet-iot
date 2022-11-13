using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBlade.API.dotnet.client.core.Models
{
    public class DeviceToGatewayModel
    {
        public string gatewayId { get; set; }
        public string deviceId { get; set; }

        public DeviceToGatewayModel()
        {
            gatewayId = string.Empty;
            deviceId = string.Empty;
        }

        public DeviceToGatewayModel(string gatewayIdIn, string deviceIdIn)
        {
            gatewayId = gatewayIdIn;
            deviceId = deviceIdIn;
        }
    }
}
