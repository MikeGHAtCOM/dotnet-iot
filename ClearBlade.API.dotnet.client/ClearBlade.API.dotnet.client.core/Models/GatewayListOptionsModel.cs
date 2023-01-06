using ClearBlade.API.dotnet.client.core.Enums;

namespace ClearBlade.API.dotnet.client.core.Models
{
    public class GatewayListOptionsModel
    {
        public GatewayTypeEnum GatewayType { get; set; }
        public string AssociationsGatewayId { get; set; }
        public string AssociationsDeviceId { get; set; }

        public GatewayListOptionsModel()
        {
            GatewayType = GatewayTypeEnum.GATEWAY_TYPE_UNSPECIFIED;
            AssociationsGatewayId = string.Empty;
            AssociationsDeviceId = string.Empty;
        }

        public GatewayListOptionsModel(GatewayTypeEnum gatewayType, string associationsGatewayId, string associationsDeviceId)
        {
            this.GatewayType = gatewayType;
            this.AssociationsGatewayId = associationsGatewayId;
            this.AssociationsDeviceId = associationsDeviceId;
        }
    }
}
