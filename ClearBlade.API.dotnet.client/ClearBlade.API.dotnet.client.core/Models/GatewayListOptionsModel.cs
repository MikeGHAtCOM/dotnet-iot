/*
 * Copyright (c) 2023 ClearBlade Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 *
 * Copyright (c) 2018 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 */
 
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
