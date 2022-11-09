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
        /// <param name="handler"></param>
        /// <param name="baseUrl"></param>
        void Initialize(HttpLoggingHandler handler, string baseUrl);
        /// <summary>
        /// Method to get list of devices.
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="parentPath"></param>
        /// <returns>List of Devices</returns>
        Task<(bool, IEnumerable<DeviceModel>)> GetDevicesList(int version, string system_key, string parentPath);
        /// <summary>
        /// A generic api to call any post method related to Devices
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="deviceName"></param>
        /// <param name="methodName"></param>
        /// <param name="body"></param>
        /// <returns>Success / Failure</returns>
        Task<bool> PostToDevice(int version, string system_key, string deviceName, string methodName, object body);
        /// <summary>
        /// Api to create new device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="deviceIn"></param>
        /// <returns>Device Model</returns>
        Task<(bool, DeviceCreateResultModel?)> CreateDevice(int version, string system_key, DeviceCreateModel deviceIn);
        /// <summary>
        /// Api to delete a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="deviceName"></param>
        /// <param name="deviceIn"></param>
        /// <returns>Error number</returns>
        Task<(bool, int?)> DeleteDevice(int version, string system_key, DeviceCreateModel deviceIn);
    }
}
