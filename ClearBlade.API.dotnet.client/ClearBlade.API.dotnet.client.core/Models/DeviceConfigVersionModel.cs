namespace ClearBlade.API.dotnet.client.core.Models
{
    public class DeviceConfigVersionModel
    {
        public string Version { get; set; }
        public DateTime CloudUpdateTime { get; set; }

        public DeviceConfigVersionModel()
        {
            Version = String.Empty;
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
