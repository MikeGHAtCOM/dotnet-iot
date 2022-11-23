using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBlade.API.dotnet.client.core.Models
{
    public class DeviceConfigVersionModel
    {
        public string version { get; set; }
        public DateTime cloudUpdateTime { get; set; }

        public DeviceConfigVersionModel()
        {
            version = String.Empty;
        }
    }
    public class DeviceConfigVersions
    {
        public List<DeviceConfigVersionModel> deviceConfigs { get; set; }

        public DeviceConfigVersions()
        {
            deviceConfigs = new List<DeviceConfigVersionModel>();
        }
    }
}
