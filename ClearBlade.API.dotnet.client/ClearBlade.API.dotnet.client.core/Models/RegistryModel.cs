using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBlade.API.dotnet.client.core.Models
{
    public class RegistryModel
    {
        public string region { get; set; }
        public string registry { get; set; }
        public string project { get; set; }

        public RegistryModel()
        {
            region = string.Empty;
            registry = string.Empty;
            project = string.Empty;
        }

        public RegistryModel(string regionIn, string registryIn, string projectIn)
        {
            region = regionIn;
            registry = registryIn;
            project = projectIn;
        }
    }
}
