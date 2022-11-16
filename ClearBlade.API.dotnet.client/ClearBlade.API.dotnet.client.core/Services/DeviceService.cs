﻿using ClearBlade.API.dotnet.client.core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    if(string.Compare(vs[i], "projects",true) == 0)
                    {
                        i++; // Next item in array will be 
                        rm.project = vs[i];
                    }
                    if (string.Compare(vs[i], "locations", true) == 0)
                    {
                        i++; // Next item in array will be 
                        rm.region = vs[i];
                    }
                    if (string.Compare(vs[i], "registries", true) == 0)
                    {
                        i++; // Next item in array will be 
                        rm.registry = vs[i];
                    }
                }

                // Further use the admin account to obtain api token etc.
                var rmKeyRes = await _adminSvc.GetRegistryCredentials(rm);
                if(!rmKeyRes.Item1 || (rmKeyRes.Item2 == null))
                    return false;

                rkm = rmKeyRes.Item2;

                HttpLoggingHandler handler = new HttpLoggingHandler(rkm.serviceAccountToken);
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
        /// <returns>List of Devices</returns>
        public async Task<(bool, IEnumerable<DeviceModel>)> GetDevicesList(int version, string parentPath)
        {
            _logger.LogInformation("Getting devices list for parent {parentPath}.", parentPath);
            if(_api == null)
                return (false, new List<DeviceModel>());
            var response = await _api.GetDevicesList(version, rkm.systemKey, parentPath);
            if (response.IsSuccessStatusCode && response.Content != null)
            {
                _logger.LogInformation("Found {y} devices", response.Content.devices.Count);
                return (true, response.Content.devices);
            }

            _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
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
        public async Task<bool> PostToDevice(int version, string deviceName, string methodName, object body)
        {
            _logger.LogInformation("Calling {method} for device {name}.", methodName, deviceName);
            if (_api == null)
                return false;
            var response = await _api.PostToDevice(version, rkm.systemKey, deviceName, methodName, body);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully completed calling method on device");
                return true;
            }

            _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
            return false;
        }

        /// <summary>
        /// Api to create new device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceIn"></param>
        /// <returns>Device Model</returns>
        public async Task<(bool, DeviceCreateResponseModel?)> CreateDevice(int version, DeviceCreateModel deviceIn)
        {
            _logger.LogInformation("Creating new device with id {id}.", deviceIn.id);
            if (_api == null)
                return (false, null);
            var response = await _api.CreateDevice(version, rkm.systemKey, deviceIn);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully created the device");
                return (true, response.Content);
            }

            _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
            return (false, null);
        }

        /// <summary>
        /// Api to delete a Device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceIn"></param>
        /// <returns></returns>
        public async Task<(bool, int?)> DeleteDevice(int version, DeviceCreateModel deviceIn)
        {
            _logger.LogInformation("Deleting device with id {id}.", deviceIn.id);
            if (_api == null)
                return (false, null);
            var response = await _api.DeleteDevice(version, rkm.systemKey, deviceIn.name, deviceIn);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully deleted the device");
                return (true, response.Content);
            }

            _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
            return (false, null);
        }

        /// <summary>
        /// Api to obtain details of a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <returns>success / failure - Device Model</returns>
        public async Task<(bool, DeviceModel?)> GetDevice(int version, string deviceName)
        {
            _logger.LogInformation("Get details of a device with name {name}.", deviceName);
            if (_api == null)
                return (false, null);
            var response = await _api.GetDevice(version, rkm.systemKey, deviceName);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully obtained the device details");
                return (true, response.Content);
            }

            _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
            return (false, null);
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
            _logger.LogInformation("Get configuration details of a device with name {name}.", deviceName);
            if (_api == null)
                return (false, null);
            var response = await _api.GetDeviceConfig(version, rkm.systemKey, deviceName, localVersion);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully obtained the device configuration details");
                return (true, response.Content);
            }

            _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
            return (false, null);
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
            _logger.LogInformation("{methodName} - device with id {id} to / fro Gateway.", methodName, body.deviceId);
            if (_api == null)
                return false;
            var response = await _api.DeviceToGateway(version, rkm.systemKey, parent, methodName, body);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully completed method {methodName}", methodName);
                return true;
            }

            _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
            return false;
        }

        /// <summary>
        /// Api to get configuration of a registry
        /// </summary>
        /// <param name="version"></param>
        /// <param name="name"></param>
        /// <returns>Success / Failure and RegistryConfigModel</returns>
        public async Task<(bool, RegistryConfigModel?)> GetRegistryConfig(int version, string name)
        {
            _logger.LogInformation("Get configuration details of a registry with name {name}.", name);
            if (_api == null)
                return (false, null);
            var response = await _api.GetRegistryConfig(version, rkm.systemKey, name);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully obtained the registry configuration details");
                return (true, response.Content);
            }

            _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
            return (false, null);
        }
    }
}
