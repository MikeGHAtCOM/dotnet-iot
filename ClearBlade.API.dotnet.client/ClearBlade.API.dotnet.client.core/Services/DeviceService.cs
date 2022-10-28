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
        private IDevicesAPIContract _api;

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
        public void Initialize(AuthHeaderHandler handler, string baseUrl)
        {
            _api = RestService.For<IDevicesAPIContract>(new HttpClient(handler)
            {
                BaseAddress = new Uri(baseUrl)
            });
        }

        /// <summary>
        /// Method to get list of devices
        /// </summary>
        /// <param name="system_key"></param>
        /// <param name="parentPath"></param>
        /// <returns>List of Devices</returns>
        public async Task<(bool, IEnumerable<DeviceModel>)> GetDevicesList(string system_key, string parentPath)
        {
            _logger.LogInformation("Getting devices list for parent {parentPath}.", parentPath);
            var response = await _api.GetDevicesList(system_key, parentPath);
            if (response.IsSuccessStatusCode && response.Content != null)
            {
                _logger.LogInformation("Found {y} devices", response.Content.devices.Count);
                return (true, response.Content.devices);
            }

            _logger.LogError(response.Error, "Reason: {ReasonPhrase}", response.ReasonPhrase);
            return (false, new List<DeviceModel>());
        }
    }
}
