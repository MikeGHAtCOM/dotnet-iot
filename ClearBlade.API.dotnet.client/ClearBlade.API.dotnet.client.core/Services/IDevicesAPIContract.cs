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
 
using ClearBlade.API.dotnet.client.core.Models;
using Refit;

namespace ClearBlade.API.dotnet.client.core.Services
{
    internal interface IDevicesApiContract
    {
        /// <summary>
        /// API contract for /cloudiot_devices api.
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="parentPath"></param>
        /// <returns>An object which contains list of device model objects</returns>
        [Get("/api/v/{version}/webhook/execute/{system_key}/cloudiot_devices")]
        Task<IApiResponse<DeviceCollection>> GetDevicesListAsync(int version, string system_key, [AliasAs("parent")] string parentPath, [AliasAs("gatewayListOptions")] GatewayListOptionsModel? gatewayOptions);

        /// <summary>
        /// A generic api to post any command related to Devices
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="deviceName"></param>
        /// <param name="methodName"></param>
        /// <param name="body"></param>
        /// <returns>Success / Failure</returns>
        [Post("/api/v/{version}/webhook/execute/{system_key}/cloudiot_devices")]
        Task<IApiResponse<bool>> PostCommandToDeviceAsync(int version, string system_key, [AliasAs("name")] string deviceName, [AliasAs("method")] string methodName, [Body] object body);

        /// <summary>
        /// A generic api to call any post method related to Devices
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="deviceName"></param>
        /// <param name="methodName"></param>
        /// <param name="body"></param>
        /// <returns>Success / Failure</returns>
        [Post("/api/v/{version}/webhook/execute/{system_key}/cloudiotdevice_devices")]
        Task<IApiResponse<bool>> PostToDeviceAsync(int version, string system_key, [AliasAs("name")] string deviceName, [AliasAs("method")] string methodName, [Body] object body);

        /// <summary>
        /// Api to create new device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="deviceIn"></param>
        /// <returns>Device Model</returns>
        [Post("/api/v/{version}/webhook/execute/{system_key}/cloudiot_devices")]
        Task<IApiResponse<DeviceCreateResponseModel>> CreateDeviceAsync(int version, string system_key, [Body] DeviceCreateModel deviceIn);

        /// <summary>
        /// Api to delete a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="deviceName"></param>
        /// <param name="deviceIn"></param>
        /// <returns>Error number</returns>
        [Delete("/api/v/{version}/webhook/execute/{system_key}/cloudiot_devices")]
        Task<IApiResponse<int>> DeleteDeviceAsync(int version, string system_key, [AliasAs("name")] string deviceName, [Body] DeviceCreateModel deviceIn);

        /// <summary>
        /// Api to obtain details of a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="deviceName"></param>
        /// <returns>Device Model</returns>
        [Get("/api/v/{version}/webhook/execute/{system_key}/cloudiot_devices")]
        Task<IApiResponse<DeviceModel>> GetDeviceAsync(int version, string system_key, [AliasAs("name")] string deviceName);

        /// <summary>
        /// Api to get configurtion details of a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="deviceName"></param>
        /// <param name="localVersion"></param>
        /// <returns>DeviceConfigResponseModel object</returns>
        [Get("/api/v/{version}/webhook/execute/{system_key}/cloudiotdevice_devices")]
        Task<IApiResponse<DeviceConfigResponseModel>> GetDeviceConfigAsync(int version, string system_key, [AliasAs("name")] string deviceName, [AliasAs("localVersion")] string localVersion);

        /// <summary>
        /// Api to bind or unbind Device to/fro gateway
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="deviceName"></param>
        /// <param name="parent"></param>
        /// <param name="methodName"></param>
        /// <param name="body"></param>
        /// <returns>Success/Failure</returns>
        [Post("/api/v/{version}/webhook/execute/{system_key}/cloudiot")]
        Task<IApiResponse<bool>> DeviceToGatewayAsync(int version, string system_key, [AliasAs("parent")] string parent, [AliasAs("method")] string methodName, [Body] object body);

        /// <summary>
        /// Api to get registry configuration
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="name"></param>
        /// <returns>RegistryConfigModel</returns>
        [Get("/api/v/{version}/webhook/execute/{system_key}/cloudiot")]
        Task<IApiResponse<RegistryConfigModel>> GetRegistryConfigAsync(int version, string system_key, [AliasAs("name")] string name);

        /// <summary>
        /// Api to update the reigstry configuration
        /// </summary>
        /// <param name="version"></param>
        /// <param name="admin_system_key"></param>
        /// <param name="registryName"></param>
        /// <param name="updateMask"></param>
        /// <param name="registryConfig"></param>
        /// <returns>RegistryConfigModel</returns>
        [Patch("/api/v/{version}/webhook/execute/{admin_system_key}/cloudiot")]
        Task<IApiResponse<RegistryConfigModel>> PatchRegistryAsync(int version, string admin_system_key, [AliasAs("name")] string registryName, [AliasAs("updateMask")] string updateMask, [Body] RegistryConfigModel registryConfig);

        /// <summary>
        /// Api to update the device configuration
        /// </summary>
        /// <param name="version"></param>
        /// <param name="admin_system_key"></param>
        /// <param name="registryName"></param>
        /// <param name="updateMask"></param>
        /// <param name="device"></param>
        /// <returns>DeviceModel</returns>
        [Patch("/api/v/{version}/webhook/execute/{admin_system_key}/cloudiot_devices")]
        Task<IApiResponse<DeviceModel>> PatchDeviceAsync(int version, string admin_system_key, [AliasAs("name")] string deviceName, [AliasAs("updateMask")] string updateMask, [Body] DeviceModel device);

        /// <summary>
        /// Api to get versions of configuration for a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="admin_system_key"></param>
        /// <param name="deviceName"></param>
        /// <param name="numVersions"></param>
        /// <returns>DeviceConfigVersions</returns>
        [Get("/api/v/{version}/webhook/execute/{admin_system_key}/cloudiot_devices_configVersions")]
        Task<IApiResponse<DeviceConfigVersions>> GetDeviceConfigVersionListAsync(int version, string admin_system_key, [AliasAs("name")] string deviceName, [AliasAs("numVersions")] int numVersions);

        /// <summary>
        /// Api to get the list of states for a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="admin_system_key"></param>
        /// <param name="deviceName"></param>
        /// <param name="numStates"></param>
        /// <returns>DeviceStateList</returns>
        [Get("/api/v/{version}/webhook/execute/{admin_system_key}/cloudiot_devices_states")]
        Task<IApiResponse<DeviceStateList>> GetDeviceStateListAsync(int version, string admin_system_key, [AliasAs("name")] string deviceName, [AliasAs("numStates")] int numStates);
    }
}
