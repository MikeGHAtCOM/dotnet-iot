using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearBlade.API.dotnet.client.core.Models;
using Refit;

namespace ClearBlade.API.dotnet.client.core.Services
{
    internal interface IDevicesAPIContract
    {
        /// <summary>
        /// API contract for /cloudiot_devices api.
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="parentPath"></param>
        /// <returns>An object which contains list of device model objects</returns>
        [Get("/api/v/{version}/webhook/execute/{system_key}/cloudiot_devices")]
        Task<IApiResponse<DeviceCollection>> GetDevicesList(int version, string system_key, [AliasAs("parent")] string parentPath);

        /// <summary>
        /// A generic api to call any post method related to Devices
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="deviceName"></param>
        /// <param name="methodName"></param>
        /// <param name="body"></param>
        /// <returns>Success / Failure</returns>
        [Post("/api/v/{version}/webhook/execute/{system_key}/cloudiot_devices")]
        Task<IApiResponse<bool>> PostToDevice(int version, string system_key, [AliasAs("name")] string deviceName, [AliasAs("method")] string methodName, [Body] object body);
    }
}
