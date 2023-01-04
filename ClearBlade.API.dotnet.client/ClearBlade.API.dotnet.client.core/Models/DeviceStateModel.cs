namespace ClearBlade.API.dotnet.client.core.Models
{
    public class DeviceStateModel
    {
        public string BinaryData { get; set; }

        public DeviceStateModel()
        {
            BinaryData = string.Empty;
        }
    }

    public class DeviceStateList
    {
        public List<DeviceStateModel> DeviceStates { get; set; }

        public DeviceStateList()
        {
            DeviceStates = new List<DeviceStateModel>();
        }
    }
}
