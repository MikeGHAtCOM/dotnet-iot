using ClearBlade.API.dotnet.client.core.Models;
using ClearBlade.API.dotnet.client.core.Services;

namespace ClearBlade.API.dotnet.client.core
{
    /// <summary>
    /// Helper class that will provide methods to be called directly from
    /// main program or any other client application
    /// </summary>
    public class MainClient
    {
        private readonly IDeviceService _deviceSvc;

        public MainClient(IDeviceService deviceSvc)
        {
            _deviceSvc = deviceSvc;
        }
        /// <summary>
        /// Helper class method to get the list of devices from ClearBlade IOT
        /// </summary>
        /// <param name="version"></param>
        /// <param name="parentPath"></param>
        /// <returns>Result true or false in the segment Item1 and actual list of devices in the
        /// segment Item2</returns>
        public async Task<(bool, IEnumerable<DeviceModel>)> GetDevicesList(int version, string parentPath)
        {
            // Initialize the service
            if (!await _deviceSvc.Initialize(parentPath))
                return (false, new List<DeviceModel>());

            // call method GetDevicesList
            var res = await _deviceSvc.GetDevicesList(version, parentPath, null);
            return res;
        }

        /// <summary>
        /// Helper class method to send command to device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <param name="body"></param>
        /// <returns>Success / Failure</returns>
        public async Task<bool> SendCommandToDevice(int version, string deviceName, object body)
        {
            // Initialize the service
            if (!await _deviceSvc.Initialize(deviceName))
                return false;

            return await _deviceSvc.PostCommandToDevice(version, deviceName, "sendCommandToDevice", body);
        }

        /// <summary>
        /// Helper class method to modify device config
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <param name="body"></param>
        /// <returns>Success / Failure</returns>
        public async Task<bool> ModifyCloudToDeviceConfig(int version, string deviceName, object body)
        {
            // Initialize the service
            if (!await _deviceSvc.Initialize(deviceName))
                return false;

            return await _deviceSvc.PostCommandToDevice(version, deviceName, "modifyCloudToDeviceConfig", body);
        }

        /// <summary>
        /// Helper class method to set state for a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <param name="body"></param>
        /// <returns>Success / Failure</returns>
        public async Task<bool> DeviceSetState(int version, string deviceName, object body)
        {
            // Initialize the service
            if (!await _deviceSvc.Initialize(deviceName))
                return false;

            return await _deviceSvc.PostToDevice(version, deviceName, "setState", body);
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
        /// <returns>Success / Failure + Device Model</returns>
        public async Task<(bool, DeviceCreateResponseModel?)> CreateDevice(int version, string deviceIdIn, string deviceNameIn)
        {
            // Initialize the service
            if (!await _deviceSvc.Initialize(deviceNameIn))
                return (false, null);

            DeviceCreateModel model = new DeviceCreateModel();
            model.Id = deviceIdIn;
            model.Name = deviceNameIn;

            return await _deviceSvc.CreateDevice(version, model);
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
        public async Task<(bool, int?)> DeleteDevice(int version, string deviceIdIn, string deviceNameIn)
        {
            // Initialize the service
            if (!await _deviceSvc.Initialize(deviceNameIn))
                return (false, null);

            DeviceCreateModel model = new DeviceCreateModel();
            model.Id = deviceIdIn;
            model.Name = deviceNameIn;

            return await _deviceSvc.DeleteDevice(version, model);
        }

        /// <summary>
        /// Helper class method to get details of a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceNameIn"></param>
        /// <returns>success / failure - Device Model</returns>
        public async Task<(bool, DeviceModel?)> GetDevice(int version, string deviceNameIn)
        {
            // Initialize the service
            if (!await _deviceSvc.Initialize(deviceNameIn))
                return (false, null);

            return await _deviceSvc.GetDevice(version, deviceNameIn);
        }

        /// <summary>
        /// Helper class method to get configuration of a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceNameIn"></param>
        /// <param name="localVersionIn"></param>
        /// <returns>Success / Failure and device configuration</returns>
        public async Task<(bool, DeviceConfigResponseModel?)> GetDeviceConfig(int version, string deviceNameIn, string localVersionIn)
        {
            // Initialize the service
            if (!await _deviceSvc.Initialize(deviceNameIn))
                return (false, null);

            return await _deviceSvc.GetDeviceConfig(version, deviceNameIn, localVersionIn);
        }

        /// <summary>
        /// Helper class method to bind a device to Gateway
        /// </summary>
        /// <param name="version"></param>
        /// <param name="parent"></param>
        /// <param name="gatewayId"></param>
        /// <param name="deviceId"></param>
        /// <returns>Success / Failure</returns>
        public async Task<bool> BindDeviceToGateway(int version, string parent, string gatewayId, string deviceId)
        {
            // Initialize the service
            if (!await _deviceSvc.Initialize(parent))
                return false;

            return await _deviceSvc.DeviceToGateway(version, parent, "bindDeviceToGateway", new DeviceToGatewayModel(gatewayId, deviceId));
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
        public async Task<bool> UnBindDeviceFromGateway(int version, string parent, string gatewayId, string deviceId)
        {
            // Initialize the service
            if (!await _deviceSvc.Initialize(parent))
                return false;

            return await _deviceSvc.DeviceToGateway(version, parent, "unbindDeviceFromGateway", new DeviceToGatewayModel(gatewayId, deviceId));
        }

        /// <summary>
        /// Helper class method to get registry configuration
        /// </summary>
        /// <param name="version"></param>
        /// <param name="name"></param>
        /// <returns>Success / Failure and RegistryConfigModel</returns>
        public async Task<(bool, RegistryConfigModel?)> GetRegistryConfig(int version, string name)
        {
            // Initialize the service
            if (!await _deviceSvc.Initialize(name))
                return (false, null);

            return await _deviceSvc.GetRegistryConfig(version, name);
        }

        /// <summary>
        /// Helper class method to update registry configuration
        /// </summary>
        /// <param name="version"></param>
        /// <param name="name"></param>
        /// <param name="updateMask"></param>
        /// <param name="registryConfig"></param>
        /// <returns>Success / Failure and RegistryConfigModel</returns>
        public async Task<(bool, RegistryConfigModel?)> PatchRegistry(int version, string name, string updateMask, RegistryConfigModel registryConfig)
        {
            // Initialize the service
            if (!await _deviceSvc.Initialize(name))
                return (false, null);

            return await _deviceSvc.PatchRegistry(version, name, updateMask, registryConfig);
        }

        /// <summary>
        /// Helper class method to update device configuration
        /// </summary>
        /// <param name="version"></param>
        /// <param name="name"></param>
        /// <param name="updateMask"></param>
        /// <param name="deviceConfig"></param>
        /// <returns>Success / Failure and DeviceModel</returns>
        public async Task<(bool, DeviceModel?)> PatchDevice(int version, string name, string updateMask, DeviceModel deviceConfig)
        {
            // Initialize the service
            if (!await _deviceSvc.Initialize(name))
                return (false, null);

            return await _deviceSvc.PatchDevice(version, name, updateMask, deviceConfig);
        }

        /// <summary>
        /// Helper class method to get device configuration versions
        /// </summary>
        /// <param name="version"></param>
        /// <param name="name"></param>
        /// <param name="numVersions"></param>
        /// <returns>Success / Failure and DeviceConfigVersions</returns>
        public async Task<(bool, DeviceConfigVersions?)> GetDeviceConfigVersionList(int version, string name, int numVersions)
        {
            // Initialize the service
            if (!await _deviceSvc.Initialize(name))
                return (false, null);

            return await _deviceSvc.GetDeviceConfigVersionList(version, name, numVersions);
        }

        /// <summary>
        /// Helper class method to get device states list
        /// </summary>
        /// <param name="version"></param>
        /// <param name="name"></param>
        /// <param name="numStates"></param>
        /// <returns>Success/Failure and DeviceStateList</returns>
        public async Task<(bool, DeviceStateList?)> GetDeviceStateList(int version, string name, int numStates)
        {
            // Initialize the service
            if (!await _deviceSvc.Initialize(name))
                return (false, null);

            return await _deviceSvc.GetDeviceStateList(version, name, numStates);
        }
    }
}