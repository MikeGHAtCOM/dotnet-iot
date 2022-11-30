using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBlade.API.dotnet.client.core.Models
{
    public class DeviceSetStateRequestModel
    {
        public DeviceStateModel state { get; set; }

        public DeviceSetStateRequestModel()
        {
            state = new DeviceStateModel();
        }
    }
}
