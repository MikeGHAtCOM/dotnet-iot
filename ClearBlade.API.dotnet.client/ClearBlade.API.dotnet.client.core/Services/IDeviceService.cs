using ClearBlade.API.dotnet.client.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Task<bool> Initialize(string parentPath);
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
        /// <returns>List of Devices</returns>
        Task<(bool, IEnumerable<DeviceModel>)> GetDevicesList(int version, string parentPath);
        /// <summary>
        /// A generic api to call any post method related to Devices
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <param name="methodName"></param>
        /// <param name="body"></param>
        /// <returns>Success / Failure</returns>
        Task<bool> PostToDevice(int version, string deviceName, string methodName, object body);
        /// <summary>
        /// Api to create new device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceIn"></param>
        /// <returns>Device Model</returns>
        Task<(bool, DeviceCreateResponseModel?)> CreateDevice(int version, DeviceCreateModel deviceIn);
        /// <summary>
        /// Api to delete a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceIn"></param>
        /// <returns>Error number</returns>
        Task<(bool, int?)> DeleteDevice(int version, DeviceCreateModel deviceIn);
        /// <summary>
        /// Api to obtain details of a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <returns>success / failure - Device Model</returns>
        Task<(bool, DeviceModel?)> GetDevice(int version, string deviceName);
        /// <summary>
        /// Api to obtain configuration details of a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="deviceName"></param>
        /// <param name="localVersion"></param>
        /// <returns>success / failure - Device config Model</returns>
        Task<(bool, DeviceConfigResponseModel?)> GetDeviceConfig(int version, string deviceName, string localVersion);
        /// <summary>
        /// Api to bind or unbind device to/from a gateway
        /// </summary>
        /// <param name="version"></param>
        /// <param name="parent"></param>
        /// <param name="methodName"></param>
        /// <param name="body"></param>
        /// <returns>Success / Failure</returns>
        Task<bool> DeviceToGateway(int version, string parent, string methodName, DeviceToGatewayModel body);

        /// <summary>
        /// Api to get configuration of a registry
        /// </summary>
        /// <param name="version"></param>
        /// <param name="name"></param>
        /// <returns>Success / Failure and RegistryConfigModel</returns>
        Task<(bool, RegistryConfigModel?)> GetRegistryConfig(int version, string name);
    }
}
