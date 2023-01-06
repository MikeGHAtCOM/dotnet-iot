namespace ClearBlade.API.dotnet.client.core.Models
{
    public class ServiceAccountDetails
    {
        public string SystemKey { get; set; }
        public string Token { get; set; }
        public string Url { get; set; }
        public string Project { get; set; }

        public ServiceAccountDetails()
        {
            SystemKey = string.Empty;
            Token = string.Empty;
            Url = string.Empty;
            Project = string.Empty;
        }
    }
}
