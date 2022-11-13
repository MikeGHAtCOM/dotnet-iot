using ClearBlade.API.dotnet.client.core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private IDevicesAPIContract? _api;

        /// <summary>
        /// Constructor which initializes logging service
        /// </summary>
        /// <param name="logger"></param>
        public DeviceService(ILogger<DeviceService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Method used to initialize the Device service. This essentially provides
        /// the base URL of the ClearBlade regional IOT and a handler that contains the
        /// authorization token
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="baseUrl"></param>
        public void Initialize(HttpLoggingHandler handler, string baseUrl)
        {
            _api = RestService.For<IDevicesAPIContract>(new HttpClient(handler)
            {
                BaseAddress = new Uri(baseUrl)
            });
        }

        /// <summary>
        /// Method to get list of devices
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="parentPath"></param>
        /// <returns>List of Devices</returns>
        public async Task<(bool, IEnumerable<DeviceModel>)> GetDevicesList(int version, string system_key, string parentPath)
        {
            _logger.LogInformation("Getting devices list for parent {parentPath}.", parentPath);
            if(_api == null)
                return (false, new List<DeviceModel>());
            var response = await _api.GetDevicesList(version, system_key, parentPath);
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
        /// <param name="system_key"></param>
        /// <param name="deviceName"></param>
        /// <param name="methodName"></param>
        /// <param name="body"></param>
        /// <returns>Success / Failure</returns>
        public async Task<bool> PostToDevice(int version, string system_key, string deviceName, string methodName, object body)
        {
            _logger.LogInformation("Calling {method} for device {name}.", methodName, deviceName);
            if (_api == null)
                return false;
            var response = await _api.PostToDevice(version, system_key, deviceName, methodName, body);
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
        /// <param name="system_key"></param>
        /// <param name="deviceIn"></param>
        /// <returns>Device Model</returns>
        public async Task<(bool, DeviceCreateResponseModel?)> CreateDevice(int version, string system_key, DeviceCreateModel deviceIn)
        {
            _logger.LogInformation("Creating new device with id {id}.", deviceIn.id);
            if (_api == null)
                return (false, null);
            var response = await _api.CreateDevice(version, system_key, deviceIn);
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
        /// <param name="system_key"></param>
        /// <param name="deviceIn"></param>
        /// <returns></returns>
        public async Task<(bool, int?)> DeleteDevice(int version, string system_key, DeviceCreateModel deviceIn)
        {
            _logger.LogInformation("Deleting device with id {id}.", deviceIn.id);
            if (_api == null)
                return (false, null);
            var response = await _api.DeleteDevice(version, system_key, deviceIn.name, deviceIn);
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
        /// <param name="system_key"></param>
        /// <param name="deviceName"></param>
        /// <returns>success / failure - Device Model</returns>
        public async Task<(bool, DeviceModel?)> GetDevice(int version, string system_key, string deviceName)
        {
            _logger.LogInformation("Get details of a device with name {name}.", deviceName);
            if (_api == null)
                return (false, null);
            var response = await _api.GetDevice(version, system_key, deviceName);
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
        /// <param name="system_key"></param>
        /// <param name="deviceName"></param>
        /// <param name="localVersion"></param>
        /// <returns>success / failure - Device config Model</returns>
        public async Task<(bool, DeviceConfigResponseModel?)> GetDeviceConfig(int version, string system_key, string deviceName, string localVersion)
        {
            _logger.LogInformation("Get configuration details of a device with name {name}.", deviceName);
            if (_api == null)
                return (false, null);
            var response = await _api.GetDeviceConfig(version, system_key, deviceName, localVersion);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully obtained the device configuration details");
                return (true, response.Content);
            }

            _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
            return (false, null);
        }

        public async Task<bool> DeviceToGateway(int version, string system_key, string parent, string methodName, DeviceToGatewayModel body)
        {
            _logger.LogInformation("{methodName} - device with id {id} to / fro Gateway.", methodName, body.deviceId);
            if (_api == null)
                return false;
            var response = await _api.DeviceToGateway(version, system_key, parent, methodName, body);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully completed method {methodName}", methodName);
                return true;
            }

            _logger.LogError(response.Error, "Reason: {ReasonPhrase}, Error {error}", response.ReasonPhrase, (response.Error == null) ? "" : response.Error.Content);
            return false;
        }
    }
}
