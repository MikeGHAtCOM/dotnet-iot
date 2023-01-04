namespace ClearBlade.API.dotnet.client.core.Models
{
    public class DeviceSetStateRequestModel
    {
        public DeviceStateModel State { get; set; }

        public DeviceSetStateRequestModel()
        {
            State = new DeviceStateModel();
        }
    }
}
