using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBlade.API.dotnet.client.core.Models
{
    public class RegistryKeyModel
    {
        public string systemKey { get; set; }
        public string serviceAccountToken { get; set; }
        public string url { get; set; }

        public RegistryKeyModel()
        {
            systemKey = string.Empty;
            serviceAccountToken = string.Empty;
            url = string.Empty;
        }
    }
}
