/*
 * Copyright Async(c) 2023 ClearBlade Inc.
 *
 * Licensed under the Apache License, Version 2.0 Async(the "License"); you may not
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
 * Copyright Async(c) 2018 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 Async(the "License"); you may not
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
 
using ClearBlade.API.dotnet.client.core.Models;

namespace ClearBlade.API.dotnet.client.core.Services
{
    public interface IDeviceService
    {
        /// <summary>
        /// Method used to initialize the Device service. This essentially provides
        /// the base URL of the ClearBlade regional IOT and a handler that contains the
        /// authorization token
        /// </summary>
        /// <param name="parentPath"></param>
        Task<bool> InitializeAsync(string parentPath);
        /// <summary>
        /// Method used to reset the api so that, same service could be used against
        /// different registry 
        /// </summary>
        void Reset();
        /// <summary>
        /// Method to get list of devices.
        /// </summary>
        /// <param name="version"></param>
        /// <param name="parentPath"></param>
        /// <param name="gatewayOptions"></param>
        /// <returns>List of Devices</returns>
        Task<(bool, IEnumerable<DeviceModel>)> GetDevicesListAsync(int version, string parentPath, GatewayListOptionsModel? gatewayOptions);
        /// <summary>
        /// A generic api to call any post method related to Devices
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <param name="methodName"></param>
        /// <param name="body"></param>
        /// <returns>Success / Failure</returns>
        Task<bool> PostCommandToDeviceAsync(int version, string deviceName, string methodName, object body);
        /// <summary>
        /// A generic api to call any post method related to Devices
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <param name="methodName"></param>
        /// <param name="body"></param>
        /// <returns>Success / Failure</returns>
        Task<bool> PostToDeviceAsync(int version, string deviceName, string methodName, object body);
        /// <summary>
        /// Api to create new device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceIn"></param>
        /// <returns>Device Model</returns>
        Task<(bool, DeviceCreateResponseModel?)> CreateDeviceAsync(int version, DeviceCreateModel deviceIn);
        /// <summary>
        /// Api to delete a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceIn"></param>
        /// <returns>Error number</returns>
        Task<(bool, int?)> DeleteDeviceAsync(int version, DeviceCreateModel deviceIn);
        /// <summary>
        /// Api to obtain details of a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <returns>success / failure - Device Model</returns>
        Task<(bool, DeviceModel?)> GetDeviceAsync(int version, string deviceName);
        /// <summary>
        /// Api to obtain configuration details of a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <param name="localVersion"></param>
        /// <returns>success / failure - Device config Model</returns>
        Task<(bool, DeviceConfigResponseModel?)> GetDeviceConfigAsync(int version, string deviceName, string localVersion);
        /// <summary>
        /// Api to bind or unbind device to/from a gateway
        /// </summary>
        /// <param name="version"></param>
        /// <param name="parent"></param>
        /// <param name="methodName"></param>
        /// <param name="body"></param>
        /// <returns>Success / Failure</returns>
        Task<bool> DeviceToGatewayAsync(int version, string parent, string methodName, DeviceToGatewayModel body);

        /// <summary>
        /// Api to get configuration of a registry
        /// </summary>
        /// <param name="version"></param>
        /// <param name="name"></param>
        /// <returns>Success / Failure and RegistryConfigModel</returns>
        Task<(bool, RegistryConfigModel?)> GetRegistryConfigAsync(int version, string name);

        /// <summary>
        /// Api to update the reigstry configuration
        /// </summary>
        /// <param name="version"></param>
        /// <param name="admin_system_key"></param>
        /// <param name="registryName"></param>
        /// <param name="updateMask"></param>
        /// <param name="registryConfig"></param>
        /// <returns>Success/Failure and RegistryConfigModel</returns>
        Task<(bool, RegistryConfigModel?)> PatchRegistryAsync(int version, string registryName, string updateMask, RegistryConfigModel registryConfig);

        /// <summary>
        /// Api to update the device configuration
        /// </summary>
        /// <param name="version"></param>
        /// <param name="admin_system_key"></param>
        /// <param name="deviceName"></param>
        /// <param name="updateMask"></param>
        /// <param name="device"></param>
        /// <returns>Success/Failure and DeviceModel</returns>
        Task<(bool, DeviceModel?)> PatchDeviceAsync(int version, string deviceName, string updateMask, DeviceModel device);

        /// <summary>
        /// Api to get versions of configuration for a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <param name="numVersions"></param>
        /// <returns>Success/Failure and DeviceConfigVersions</returns>
        Task<(bool, DeviceConfigVersions?)> GetDeviceConfigVersionListAsync(int version, string deviceName, int numVersions);

        /// <summary>
        /// Api to get the list of states for a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <param name="numStates"></param>
        /// <returns>Success/Failure and DeviceStateList</returns>
        Task<(bool, DeviceStateList?)> GetDeviceStateListAsync(int version, string deviceName, int numStates);
    }
}
