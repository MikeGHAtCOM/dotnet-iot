using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBlade.API.dotnet.client.core.Models
{
    public class DeviceStateModel
    {
        public string binaryData { get; set; }

        public DeviceStateModel()
        {
            binaryData = string.Empty;
        }
    }

    public class DeviceStateList
    {
        public List<DeviceStateModel> deviceStates { get; set; }

        public DeviceStateList()
        {
            deviceStates = new List<DeviceStateModel>();
        }
    }
}
