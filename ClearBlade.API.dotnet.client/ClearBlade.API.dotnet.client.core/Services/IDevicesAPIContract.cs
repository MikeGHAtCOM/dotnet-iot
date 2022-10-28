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
        /// <param name="system_key"></param>
        /// <param name="parentPath"></param>
        /// <returns>An object which contains list of device model objects</returns>
        [Get("/api/v/4/webhook/execute/{system_key}/cloudiot_devices")]
        Task<IApiResponse<DeviceCollection>> GetDevicesList(string system_key, [AliasAs("parent")] string parentPath);
    }
}
