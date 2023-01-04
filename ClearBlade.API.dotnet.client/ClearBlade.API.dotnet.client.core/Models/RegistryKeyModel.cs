namespace ClearBlade.API.dotnet.client.core.Models
{
    public class RegistryKeyModel
    {
        public string SystemKey { get; set; }
        public string ServiceAccountToken { get; set; }
        public string url { get; set; }

        public RegistryKeyModel()
        {
            SystemKey = string.Empty;
            ServiceAccountToken = string.Empty;
            url = string.Empty;
        }
    }
}
