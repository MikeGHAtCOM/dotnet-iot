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
        /// <param name="baseUrl"></param>
        /// <param name="system_key"></param>
        /// <param name="accessToken"></param>
        /// <param name="parentPath"></param>
        /// <returns>Result true or false in the segment Item1 and actual list of devices in the
        /// segment Item2</returns>
        public async Task<(bool, IEnumerable<DeviceModel>)> GetDevicesList(string baseUrl, string system_key, string accessToken, string parentPath)
        {
            // Initialize the service
            _deviceSvc.Initialize(new AuthHeaderHandler(accessToken), baseUrl);

            // call method GetDevicesList
            var res =  await _deviceSvc.GetDevicesList(system_key, parentPath);
            return res;
        }
    }
}