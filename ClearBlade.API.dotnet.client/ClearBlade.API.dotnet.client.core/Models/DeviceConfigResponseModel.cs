namespace ClearBlade.API.dotnet.client.core.Models
{
    public class DeviceConfigResponseModel
    {
        public string Version { get; set; }
        public string BinaryData { get; set; }

        public DeviceConfigResponseModel()
        {
            Version = string.Empty;
            BinaryData = string.Empty;
        }
    }
}
