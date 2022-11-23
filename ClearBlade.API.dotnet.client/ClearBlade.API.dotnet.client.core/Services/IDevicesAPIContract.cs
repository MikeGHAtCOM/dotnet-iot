using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        /// <summary>
        /// Api to create new device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="deviceIn"></param>
        /// <returns>Device Model</returns>
        [Post("/api/v/{version}/webhook/execute/{system_key}/cloudiot_devices")]
        Task<IApiResponse<DeviceCreateResponseModel>> CreateDevice(int version, string system_key, [Body] DeviceCreateModel deviceIn);

        /// <summary>
        /// Api to delete a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="deviceName"></param>
        /// <param name="deviceIn"></param>
        /// <returns>Error number</returns>
        [Delete("/api/v/{version}/webhook/execute/{system_key}/cloudiot_devices")]
        Task<IApiResponse<int>> DeleteDevice(int version, string system_key, [AliasAs("name")] string deviceName, [Body] DeviceCreateModel deviceIn);

        /// <summary>
        /// Api to obtain details of a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="deviceName"></param>
        /// <returns>Device Model</returns>
        [Get("/api/v/{version}/webhook/execute/{system_key}/cloudiot_devices")]
        Task<IApiResponse<DeviceModel>> GetDevice(int version, string system_key, [AliasAs("name")] string deviceName);

        /// <summary>
        /// Api to get configurtion details of a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="deviceName"></param>
        /// <param name="localVersion"></param>
        /// <returns>DeviceConfigResponseModel object</returns>
        [Get("/api/v/{version}/webhook/execute/{system_key}/cloudiotdevice_devices")]
        Task<IApiResponse<DeviceConfigResponseModel>> GetDeviceConfig(int version, string system_key, [AliasAs("name")] string deviceName, [AliasAs("localVersion")] string localVersion);

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
        Task<IApiResponse<bool>> DeviceToGateway(int version, string system_key, [AliasAs("parent")] string parent, [AliasAs("method")] string methodName, [Body] object body);

        /// <summary>
        /// Api to get registry configuration
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="name"></param>
        /// <returns>RegistryConfigModel</returns>
        [Get("/api/v/{version}/webhook/execute/{system_key}/cloudiot")]
        Task<IApiResponse<RegistryConfigModel>> GetRegistryConfig(int version, string system_key, [AliasAs("name")] string name);

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
        Task<IApiResponse<RegistryConfigModel>> PatchRegistry(int version, string admin_system_key, [AliasAs("name")] string registryName, [AliasAs("updateMask")] string updateMask, [Body] RegistryConfigModel registryConfig);

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
        Task<IApiResponse<DeviceModel>> PatchDevice(int version, string admin_system_key, [AliasAs("name")] string deviceName, [AliasAs("updateMask")] string updateMask, [Body] DeviceModel device);

        /// <summary>
        /// Api to get versions of configuration for a device
        /// </summary>
        /// <param name="version"></param>
        /// <param name="admin_system_key"></param>
        /// <param name="deviceName"></param>
        /// <param name="numVersions"></param>
        /// <returns>DeviceConfigVersions</returns>
        [Get("/api/v/{version}/webhook/execute/{admin_system_key}/cloudiot_devices_configVersions")]
        Task<IApiResponse<DeviceConfigVersions>> GetDeviceConfigVersionList(int version, string admin_system_key, [AliasAs("name")] string deviceName, [AliasAs("numVersions")] int numVersions);
    }
}
