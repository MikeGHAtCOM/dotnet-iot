namespace ClearBlade.API.dotnet.client.core.Models
{
    public class RegistryModel
    {
        public string Region { get; set; }
        public string Registry { get; set; }
        public string Project { get; set; }

        public RegistryModel()
        {
            Region = string.Empty;
            Registry = string.Empty;
            Project = string.Empty;
        }

        public RegistryModel(string regionIn, string registryIn, string projectIn)
        {
            Region = regionIn;
            Registry = registryIn;
            Project = projectIn;
        }
    }
}
