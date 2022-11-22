using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBlade.API.dotnet.client.core.Models
{
    public class DeviceConfigResponseModel
    {
        public string version { get; set; }
        public string binaryData { get; set; }

        public DeviceConfigResponseModel()
        {
            version = string.Empty;
            binaryData = string.Empty;
        }
    }
}
