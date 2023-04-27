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
#pragma warning disable IDE1006 // Naming Styles
        public List<DeviceConfigVersionModel> deviceConfigs { get; set; }
#pragma warning restore IDE1006 // Naming Styles

        public DeviceConfigVersions()
        {
            deviceConfigs = new List<DeviceConfigVersionModel>();
        }
    }
}
