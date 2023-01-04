namespace ClearBlade.API.dotnet.client.core.Models
{
    public class DeviceToGatewayModel
    {
        public string GatewayId { get; set; }
        public string DeviceId { get; set; }

        public DeviceToGatewayModel()
        {
            GatewayId = string.Empty;
            DeviceId = string.Empty;
        }

        public DeviceToGatewayModel(string gatewayIdIn, string deviceIdIn)
        {
            GatewayId = gatewayIdIn;
            DeviceId = deviceIdIn;
        }
    }
}
