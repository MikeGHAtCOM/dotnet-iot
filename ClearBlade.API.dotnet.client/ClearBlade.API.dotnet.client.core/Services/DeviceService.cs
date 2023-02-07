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
using Microsoft.Extensions.Logging;
using Refit;

namespace ClearBlade.API.dotnet.client.core.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly ILogger<DeviceService> _logger;
        private IDevicesApiContract? _api;
        private readonly IAdminService _adminSvc;
        private RegistryKeyModel rkm;

        /// <summary>
        /// Constructor which initializes logging service
        /// </summary>
        /// <param name="logger"></param>
        public DeviceService(ILogger<DeviceService> logger, IAdminService adminSvc)
        {
            _logger = logger;
            _api = null;
            _adminSvc = adminSvc;
            rkm = new RegistryKeyModel();
        }

        /// <summary>
        /// Method used to initialize the Device service. This essentially provides
        /// the base URL of the ClearBlade regional IOT and a handler that contains the
        /// authorization token
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="baseUrl"></param>
        public async Task<bool> Initialize(string parentPath)
        {
            bool bRetVal = true;

            if (_api != null)
            {
                //already initialized
                return bRetVal;
            }

            try
            {
                // Get registry details from the parent path
                RegistryModel rm = new RegistryModel();
                string[] vs = parentPath.Split('/');
                if (vs.Length < 6)
                {
                    _logger.LogError("Incorrect Parent path found. Please use format \"projects/[PROJECT]/locations/[LOCATION]/registries/[REGISTRY]\" ");
                    return false;
                }

                for (int i = 0; i < vs.Length; i++)
                {
                    if (string.Compare(vs[i], "projects", true) == 0)
                    {
                        i++; // Next item in array will be 
                        rm.Project = vs[i];
                    }
                    if (string.Compare(vs[i], "locations", true) == 0)
                    {
                        i++; // Next item in array will be 
                        rm.Region = vs[i];
                    }
                    if (string.Compare(vs[i], "registries", true) == 0)
                    {
                        i++; // Next item in array will be 
                        rm.Registry = vs[i];
                    }
                }

                // Further use the admin account to obtain api token etc.
                var rmKeyRes = await _adminSvc.GetRegistryCredentials(rm);
                if (!rmKeyRes.Item1 || (rmKeyRes.Item2 == null))
                    return false;

                rkm = rmKeyRes.Item2;

                HttpLoggingHandler handler = new HttpLoggingHandler(rkm.ServiceAccountToken);
                string baseUrl = rkm.url;
                _api = RestService.For<IDevicesApiContract>(new HttpClient(handler)
                {
                    BaseAddress = new Uri(baseUrl)
                });
            }
            catch (Exception ee)
            {
                _logger.LogError(ee, "System Error while initializing Device Service. Message: ", ee.Message);
            }

            return bRetVal;
        }

        /// <summary>
        /// Method used to reset the api so that, same service could be used against
        /// different registry 
        /// </summary>
        public void Reset()
        {
            _api = null;
        }

        /// <summary>
        /// Method to get list of devices
        /// </summary>
        /// <param name="version"></param>
        /// <param name="parentPath"></param>
        /// <param name="gatewayOptions"></param>
        /// <returns>List of Devices</returns>
        public async Task<(bool, IEnumerable<DeviceModel>)> GetDevicesList(int version, string parentPath, GatewayListOptionsModel? gatewayOptions)
        {
            try
            {
                _logger.LogInformation("Getting devices list for parent {parentPath}.", parentPath);
                if (_api == null)
                    return (false, new List<DeviceModel>());
                var response = await _api.GetDevicesList(version, rkm.SystemKey, parentPath, gatewayOptions);
                if (response.IsSuccessStatusCode && response.Content != null)
                {
                    _logger.LogInformation("Found {y} devices", response.Content.Devices.Count);
                    return (true, response.Content.Devices);
                }
                _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
                return (false, new List<DeviceModel>());
            }
            catch (Exception ee)
            {
                _logger.LogError(ee, "System Error while getting the device list. Message: ", ee.Message);
            }

            _logger.LogError("Error while getting the device list");
            return (false, new List<DeviceModel>());
        }

        /// <summary>
        /// Method to get list of devices.
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <param name="methodName"></param>
        /// <param name="body"></param>
        /// <returns>Success / Failure</returns>
        public async Task<bool> PostCommandToDevice(int version, string deviceName, string methodName, object body)
        {
            try
            {
                _logger.LogInformation("Calling {method} for device {name}.", methodName, deviceName);
                if (_api == null)
                    return false;
                var response = await _api.PostCommandToDevice(version, rkm.SystemKey, deviceName, methodName, body);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully completed calling method on device");
                    return true;
                }
                _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
                return false;
            }
            catch (Exception ee)
            {
                _logger.LogError(ee, "System Error while posting command to Device. Message: ", ee.Message);
                return false;
            }
        }

        /// <summary>
        /// A generic api to call any post method related to Devices
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <param name="methodName"></param>
        /// <param name="body"></param>
        /// <returns>Success / Failure</returns>
        public async Task<bool> PostToDevice(int version, string deviceName, string methodName, object body)
        {
            try
            {
                _logger.LogInformation("Calling {method} for device {name}.", methodName, deviceName);
                if (_api == null)
                    return false;
                var response = await _api.PostToDevice(version, rkm.SystemKey, deviceName, methodName, body);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully completed calling method on device");
                    return true;
                }
                _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
                return false;
            }
            catch (Exception ee)
            {
                _logger.LogError(ee, "System Error while posting to Device. Message: ", ee.Message);
                return false;
            }
        }

        /// <summary>
        /// Api to create new device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceIn"></param>
        /// <returns>Device Model</returns>
        public async Task<(bool, DeviceCreateResponseModel?)> CreateDevice(int version, DeviceCreateModel deviceIn)
        {
            try
            {
                _logger.LogInformation("Creating new device with id {id}.", deviceIn.Id);
                if (_api == null)
                    return (false, null);
                var response = await _api.CreateDevice(version, rkm.SystemKey, deviceIn);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully created the device");
                    return (true, response.Content);
                }

                _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
                return (false, null);

            }
            catch (Exception ee)
            {
                _logger.LogError(ee, "System Error while creating a Device. Message: ", ee.Message);
                return (false, null);
            }
        }

        /// <summary>
        /// Api to delete a Device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceIn"></param>
        /// <returns></returns>
        public async Task<(bool, int?)> DeleteDevice(int version, DeviceCreateModel deviceIn)
        {
            try
            {
                _logger.LogInformation("Deleting device with id {id}.", deviceIn.Id);
                if (_api == null)
                    return (false, null);
                var response = await _api.DeleteDevice(version, rkm.SystemKey, deviceIn.Name, deviceIn);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully deleted the device");
                    return (true, response.Content);
                }

                _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
                return (false, null);
            }
            catch (Exception ee)
            {
                _logger.LogError(ee, "System Error while deleting a Device. Message: ", ee.Message);
                return (false, null);
            }
        }

        /// <summary>
        /// Api to obtain details of a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <returns>success / failure - Device Model</returns>
        public async Task<(bool, DeviceModel?)> GetDevice(int version, string deviceName)
        {
            try
            {
                _logger.LogInformation("Get details of a device with name {name}.", deviceName);
                if (_api == null)
                    return (false, null);
                var response = await _api.GetDevice(version, rkm.SystemKey, deviceName);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully obtained the device details");
                    return (true, response.Content);
                }

                _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
                return (false, null);

            }
            catch (Exception ee)
            {
                _logger.LogError(ee, "System Error while obtaining device details. Message: ", ee.Message);
                return (false, null);
            }
        }

        /// <summary>
        /// Api to obtain configuration details of a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <param name="localVersion"></param>
        /// <returns>success / failure - Device config Model</returns>
        public async Task<(bool, DeviceConfigResponseModel?)> GetDeviceConfig(int version, string deviceName, string localVersion)
        {
            try
            {
                _logger.LogInformation("Get configuration details of a device with name {name}.", deviceName);
                if (_api == null)
                    return (false, null);
                var response = await _api.GetDeviceConfig(version, rkm.SystemKey, deviceName, localVersion);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully obtained the device configuration details");
                    return (true, response.Content);
                }

                _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
                return (false, null);
            }
            catch (Exception ee)
            {
                _logger.LogError(ee, "System Error while getting device configuration details. Message: ", ee.Message);
                return (false, null);
            }
        }

        /// <summary>
        /// Api to bind or unbind device to/from a gateway
        /// </summary>
        /// <param name="version"></param>
        /// <param name="parent"></param>
        /// <param name="methodName"></param>
        /// <param name="body"></param>
        /// <returns>Success / Failure</returns>
        public async Task<bool> DeviceToGateway(int version, string parent, string methodName, DeviceToGatewayModel body)
        {
            try
            {
                _logger.LogInformation("{methodName} - device with id {id} to / fro Gateway.", methodName, body.DeviceId);
                if (_api == null)
                    return false;
                var response = await _api.DeviceToGateway(version, rkm.SystemKey, parent, methodName, body);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully completed method {methodName}", methodName);
                    return true;
                }

                _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
                return false;
            }
            catch (Exception ee)
            {
                _logger.LogError(ee, "System Error while running device method. Message: ", ee.Message);
                return false;
            }
        }

        /// <summary>
        /// Api to get configuration of a registry
        /// </summary>
        /// <param name="version"></param>
        /// <param name="name"></param>
        /// <returns>Success / Failure and RegistryConfigModel</returns>
        public async Task<(bool, RegistryConfigModel?)> GetRegistryConfig(int version, string name)
        {
            try
            {
                _logger.LogInformation("Get configuration details of a registry with name {name}.", name);
                if (_api == null)
                    return (false, null);
                var response = await _api.GetRegistryConfig(version, rkm.SystemKey, name);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully obtained the registry configuration details");
                    return (true, response.Content);
                }

                _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
                return (false, null);
            }
            catch (Exception ee)
            {
                _logger.LogError(ee, "System Error while getting registry configuration. Message: ", ee.Message);
                return (false, null);
            }
        }

        /// <summary>
        /// Api to update the reigstry configuration
        /// </summary>
        /// <param name="version"></param>
        /// <param name="registryName"></param>
        /// <param name="updateMask"></param>
        /// <param name="registryConfig"></param>
        /// <returns>Success/Failure and RegistryConfigModel</returns>
        public async Task<(bool, RegistryConfigModel?)> PatchRegistry(int version, string registryName, string updateMask, RegistryConfigModel registryConfig)
        {
            try
            {
                _logger.LogInformation("Update configuration details of a registry with name {name}.", registryName);
                if (_api == null)
                    return (false, null);
                var response = await _api.PatchRegistry(version, rkm.SystemKey, registryName, updateMask, registryConfig);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully updated the registry configuration details");
                    return (true, response.Content);
                }

                _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
                return (false, null);
            }
            catch (Exception ee)
            {
                _logger.LogError(ee, "System Error while updating the registry configuration. Message: ", ee.Message);
                return (false, null);
            }
        }

        /// <summary>
        /// Api to update the device configuration
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <param name="updateMask"></param>
        /// <param name="deviceConfig"></param>
        /// <returns>Success/Failure and DeviceModel</returns>
        public async Task<(bool, DeviceModel?)> PatchDevice(int version, string deviceName, string updateMask, DeviceModel deviceConfig)
        {
            try
            {
                _logger.LogInformation("Update configuration details of a device with name {name}.", deviceName);
                if (_api == null)
                    return (false, null);
                var response = await _api.PatchDevice(version, rkm.SystemKey, deviceName, updateMask, deviceConfig);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully updated the device configuration details");
                    return (true, response.Content);
                }

                _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
                return (false, null);
            }
            catch (Exception ee)
            {
                _logger.LogError(ee, "System Error while updating the device configuration. Message: ", ee.Message);
                return (false, null);
            }
        }

        /// <summary>
        /// Api to get versions of configuration for a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <param name="numVersions"></param>
        /// <returns>Success/Failure and DeviceConfigVersions</returns>
        public async Task<(bool, DeviceConfigVersions?)> GetDeviceConfigVersionList(int version, string deviceName, int numVersions)
        {
            try
            {
                _logger.LogInformation("Get configuration versions list of a device with name {name}.", deviceName);
                if (_api == null)
                    return (false, null);
                var response = await _api.GetDeviceConfigVersionList(version, rkm.SystemKey, deviceName, numVersions);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully obtained the device configuration versions list");
                    return (true, response.Content);
                }

                _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
                return (false, null);
            }
            catch (Exception ee)
            {
                _logger.LogError(ee, "System Error while getting the device configuration versions. Message: ", ee.Message);
                return (false, null);
            }
        }

        /// <summary>
        /// Api to get the list of states for a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <param name="numStates"></param>
        /// <returns>Success/Failure and DeviceStateList</returns>
        public async Task<(bool, DeviceStateList?)> GetDeviceStateList(int version, string deviceName, int numStates)
        {
            try
            {
                _logger.LogInformation("Get states list of a device with name {name}.", deviceName);
                if (_api == null)
                    return (false, null);
                var response = await _api.GetDeviceStateList(version, rkm.SystemKey, deviceName, numStates);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully obtained the device states list");
                    return (true, response.Content);
                }

                _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
                return (false, null);
            }
            catch (Exception ee)
            {
                _logger.LogError(ee, "System Error while getting the device states. Message: ", ee.Message);
                return (false, null);
            }
        }
    }
}
