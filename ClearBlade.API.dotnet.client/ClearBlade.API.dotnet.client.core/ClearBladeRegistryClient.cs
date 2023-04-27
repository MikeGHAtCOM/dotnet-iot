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
using ClearBlade.API.dotnet.client.core.Services;

namespace ClearBlade.API.dotnet.client.core
{
    /// <summary>
    /// Helper class that will provide methods to be called directly from
    /// main program or any other client application
    /// </summary>
    public class ClearBladeRegistryClient
    {
        private readonly IDeviceService _deviceSvc;
        private readonly IRegistryService _registrySvc;

        private static readonly int clientDesignedForVersion = 4;

        public static int ClientDesignedForVersion {  get {  return clientDesignedForVersion; } }

        public ClearBladeRegistryClient(IDeviceService deviceSvc, IRegistryService registrySvc)
        {
            _deviceSvc = deviceSvc;
            _registrySvc = registrySvc;
        }
        /// <summary>
        /// Helper class method to get the list of devices from ClearBlade IOT
        /// </summary>
        /// <param name="version"></param>
        /// <param name="parentPath"></param>
        /// <returns>Result true or false in the segment Item1 and actual list of devices in the
        /// segment Item2</returns>
        public async Task<(bool, IEnumerable<DeviceModel>)> GetDevicesListAsync(int version, string parentPath, GatewayListOptionsModel? gatewayOptions)
        {
            // Initialize the service
            if (!await _deviceSvc.InitializeAsync(parentPath))
                return (false, new List<DeviceModel>());

            // call method GetDevicesList
            var res = await _deviceSvc.GetDevicesListAsync(version, parentPath, gatewayOptions);
            return res;
        }

        /// <summary>
        /// Helper class method to send command to device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <param name="body"></param>
        /// <returns>Success / Failure</returns>
        public async Task<bool> SendCommandToDeviceAsync(int version, string deviceName, object body)
        {
            // Initialize the service
            if (!await _deviceSvc.InitializeAsync(deviceName))
                return false;

            return await _deviceSvc.PostCommandToDeviceAsync(version, deviceName, "sendCommandToDevice", body);
        }

        /// <summary>
        /// Helper class method to modify device config
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <param name="body"></param>
        /// <returns>Success / Failure</returns>
        public async Task<bool> ModifyCloudToDeviceConfigAsync(int version, string deviceName, object body)
        {
            // Initialize the service
            if (!await _deviceSvc.InitializeAsync(deviceName))
                return false;

            return await _deviceSvc.PostCommandToDeviceAsync(version, deviceName, "modifyCloudToDeviceConfig", body);
        }

        /// <summary>
        /// Helper class method to set state for a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <param name="body"></param>
        /// <returns>Success / Failure</returns>
        public async Task<bool> DeviceSetStateAsync(int version, string deviceName, object body)
        {
            // Initialize the service
            if (!await _deviceSvc.InitializeAsync(deviceName))
                return false;

            return await _deviceSvc.PostToDeviceAsync(version, deviceName, "setState", body);
        }

        /// <summary>
        /// Helper class method to Create a new device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="baseUrl"></param>
        /// <param name="system_key"></param>
        /// <param name="accessToken"></param>
        /// <param name="deviceIdIn"></param>
        /// <param name="deviceNameIn"></param>
        /// <param name="credentials"></param>
        /// <returns>Success / Failure + Device Model</returns>
        public async Task<(bool, DeviceCreateResponseModel?)> CreateDeviceAsync(int version, string deviceIdIn, string deviceNameIn, List<DeviceCredential>? credentials, GatewayConfig? gatewayConfig)
        {
            // Initialize the service
            if (!await _deviceSvc.InitializeAsync(deviceNameIn))
                return (false, null);

#pragma warning disable IDE0090 // Use 'new(...)'
            DeviceCreateModel model = new DeviceCreateModel
            {
                Id = deviceIdIn,
                Name = deviceNameIn
            };
#pragma warning restore IDE0090 // Use 'new(...)'
            model.Credentials = credentials ?? model.Credentials;
            model.GatewayConfig = gatewayConfig ?? model.GatewayConfig;

            return await _deviceSvc.CreateDeviceAsync(version, model);
        }

        /// <summary>
        /// Helper class method to Delete a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="baseUrl"></param>
        /// <param name="system_key"></param>
        /// <param name="accessToken"></param>
        /// <param name="deviceIdIn"></param>
        /// <param name="deviceNameIn"></param>
        /// <returns>Success / Failure + Device Model</returns>
        public async Task<(bool, int?)> DeleteDeviceAsync(int version, string deviceIdIn, string deviceNameIn)
        {
            // Initialize the service
            if (!await _deviceSvc.InitializeAsync(deviceNameIn))
                return (false, null);

#pragma warning disable IDE0090 // Use 'new(...)'
            DeviceCreateModel model = new DeviceCreateModel
            {
                Id = deviceIdIn,
                Name = deviceNameIn
            };
#pragma warning restore IDE0090 // Use 'new(...)'

            return await _deviceSvc.DeleteDeviceAsync(version, model);
        }

        /// <summary>
        /// Helper class method to get details of a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceNameIn"></param>
        /// <returns>success / failure - Device Model</returns>
        public async Task<(bool, DeviceModel?)> GetDeviceAsync(int version, string deviceNameIn)
        {
            // Initialize the service
            if (!await _deviceSvc.InitializeAsync(deviceNameIn))
                return (false, null);

            return await _deviceSvc.GetDeviceAsync(version, deviceNameIn);
        }

        /// <summary>
        /// Helper class method to get configuration of a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceNameIn"></param>
        /// <param name="localVersionIn"></param>
        /// <returns>Success / Failure and device configuration</returns>
        public async Task<(bool, DeviceConfigResponseModel?)> GetDeviceConfigAsync(int version, string deviceNameIn, string localVersionIn)
        {
            // Initialize the service
            if (!await _deviceSvc.InitializeAsync(deviceNameIn))
                return (false, null);

            return await _deviceSvc.GetDeviceConfigAsync(version, deviceNameIn, localVersionIn);
        }

        /// <summary>
        /// Helper class method to bind a device to Gateway
        /// </summary>
        /// <param name="version"></param>
        /// <param name="parent"></param>
        /// <param name="gatewayId"></param>
        /// <param name="deviceId"></param>
        /// <returns>Success / Failure</returns>
        public async Task<bool> BindDeviceToGatewayAsync(int version, string parent, string gatewayId, string deviceId)
        {
            // Initialize the service
            if (!await _deviceSvc.InitializeAsync(parent))
                return false;

            return await _deviceSvc.DeviceToGatewayAsync(version, parent, "bindDeviceToGateway", new DeviceToGatewayModel(gatewayId, deviceId));
        }

        /// <summary>
        /// Helper class method to unbind a device from Gateway
        /// </summary>
        /// <param name="version"></param>
        /// <param name="baseUrl"></param>
        /// <param name="system_key"></param>
        /// <param name="accessToken"></param>
        /// <param name="parent"></param>
        /// <param name="gatewayId"></param>
        /// <param name="deviceId"></param>
        /// <returns>Success / Failure</returns>
        public async Task<bool> UnBindDeviceFromGatewayAsync(int version, string parent, string gatewayId, string deviceId)
        {
            // Initialize the service
            if (!await _deviceSvc.InitializeAsync(parent))
                return false;

            return await _deviceSvc.DeviceToGatewayAsync(version, parent, "unbindDeviceFromGateway", new DeviceToGatewayModel(gatewayId, deviceId));
        }

        /// <summary>
        /// Helper class method to get registry configuration
        /// </summary>
        /// <param name="version"></param>
        /// <param name="name"></param>
        /// <returns>Success / Failure and RegistryConfigModel</returns>
        public async Task<(bool, RegistryConfigModel?)> GetRegistryConfigAsync(int version, string name)
        {
            // Initialize the service
            if (!await _deviceSvc.InitializeAsync(name))
                return (false, null);

            return await _deviceSvc.GetRegistryConfigAsync(version, name);
        }

        /// <summary>
        /// Helper class method to update registry configuration
        /// </summary>
        /// <param name="version"></param>
        /// <param name="name"></param>
        /// <param name="updateMask"></param>
        /// <param name="registryConfig"></param>
        /// <returns>Success / Failure and RegistryConfigModel</returns>
        public async Task<(bool, RegistryConfigModel?)> PatchRegistryAsync(int version, string name, string updateMask, RegistryConfigModel registryConfig)
        {
            // Initialize the service
            if (!await _deviceSvc.InitializeAsync(name))
                return (false, null);

            return await _deviceSvc.PatchRegistryAsync(version, name, updateMask, registryConfig);
        }

        /// <summary>
        /// Helper class method to update device configuration
        /// </summary>
        /// <param name="version"></param>
        /// <param name="name"></param>
        /// <param name="updateMask"></param>
        /// <param name="deviceConfig"></param>
        /// <returns>Success / Failure and DeviceModel</returns>
        public async Task<(bool, DeviceModel?)> PatchDeviceAsync(int version, string name, string updateMask, DeviceModel deviceConfig)
        {
            // Initialize the service
            if (!await _deviceSvc.InitializeAsync(name))
                return (false, null);

            return await _deviceSvc.PatchDeviceAsync(version, name, updateMask, deviceConfig);
        }

        /// <summary>
        /// Helper class method to get device configuration versions
        /// </summary>
        /// <param name="version"></param>
        /// <param name="name"></param>
        /// <param name="numVersions"></param>
        /// <returns>Success / Failure and DeviceConfigVersions</returns>
        public async Task<(bool, DeviceConfigVersions?)> GetDeviceConfigVersionListAsync(int version, string name, int numVersions)
        {
            // Initialize the service
            if (!await _deviceSvc.InitializeAsync(name))
                return (false, null);

            return await _deviceSvc.GetDeviceConfigVersionListAsync(version, name, numVersions);
        }

        /// <summary>
        /// Helper class method to get device states list
        /// </summary>
        /// <param name="version"></param>
        /// <param name="name"></param>
        /// <param name="numStates"></param>
        /// <returns>Success/Failure and DeviceStateList</returns>
        public async Task<(bool, DeviceStateList?)> GetDeviceStateListAsync(int version, string name, int numStates)
        {
            // Initialize the service
            if (!await _deviceSvc.InitializeAsync(name))
                return (false, null);

            return await _deviceSvc.GetDeviceStateListAsync(version, name, numStates);
        }

        /// <summary>
        /// Helper class method to create a registry
        /// </summary>
        /// <param name="version"></param>
        /// <param name="parentPath"></param>
        /// <param name="registryConfigModel"></param>
        /// <returns></returns>
        public async Task<(bool, RegistryConfigModel?)> CreateRegistryAsync(int version, string parentPath, RegistryConfigModel registryConfigModel)
        {
            // Initialize the service
            if (!_registrySvc.Initialize())
                return (false, null);

            return await _registrySvc.CreateRegistryAsync(version, parentPath, registryConfigModel);
        }

        /// <summary>
        /// Helper class method to delete a registry
        /// </summary>
        /// <param name="version"></param>
        /// <param name="registryName"></param>
        /// <returns></returns>
        public async Task<bool> DeleteRegistryAsync(int version, string registryName)
        {
            // Initialize the service
            if (!_registrySvc.Initialize())
                return false;

            return await _registrySvc.DeleteRegistryAsync(version, registryName);
        }
    }
}