using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBlade.API.dotnet.client.core.Models
{
    public class ServiceAccountDetails
    {
        public string systemKey { get; set; }
        public string token { get; set; }
        public string url { get; set; }
        public string project { get; set; }

        public ServiceAccountDetails()
        {
            systemKey = string.Empty;
            token = string.Empty;
            url = string.Empty;
            project = string.Empty;
        }
    }
}
