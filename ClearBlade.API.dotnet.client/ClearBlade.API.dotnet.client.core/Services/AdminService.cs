﻿using ClearBlade.API.dotnet.client.core.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Refit;

namespace ClearBlade.API.dotnet.client.core.Services
{
    public class AdminService : IAdminService
    {
        private readonly ILogger<DeviceService> _logger;
        private IAdminServiceContract? _api;
        private ServiceAccountDetails? _accountDetails;

        public AdminService(ILogger<DeviceService> logger)
        {
            _logger = logger;
            _api = null;
        }

        /// <summary>
        /// Method used to initialize the admin service. This essentially provides
        /// the base URL of the ClearBlade regional IOT and a handler that contains the
        /// authorization token
        /// </summary>
        public bool Initialize()
        {
            bool bRetVal = true;

            try
            {
                // First get the location of Service account private key json
                // using windows environment (system) variable named "CLEARBLADE_CONFIGURATION"
                var jsonPath = System.Environment.GetEnvironmentVariable("CLEARBLADE_CONFIGURATION", EnvironmentVariableTarget.Machine);
                if (string.IsNullOrEmpty(jsonPath))
                {
                    _logger.LogError("Failed to get value of Windows system environment variable \"CLEARBLADE_CONFIGURATION\"");

                    return false;
                }

                // Read the details of service account
                _accountDetails = JsonConvert.DeserializeObject<ServiceAccountDetails>(System.IO.File.ReadAllText(jsonPath));
                if (_accountDetails == null)
                {
                    _logger.LogError("Failed to load service account credentials from json");

                    return false;
                }

                HttpLoggingHandler handler = new HttpLoggingHandler(_accountDetails.token);
                string baseUrl = _accountDetails.url;
                _api = RestService.For<IAdminServiceContract>(new HttpClient(handler)
                {
                    BaseAddress = new Uri(_accountDetails.url)
                });
            }
            catch (Exception ee)
            {
                _logger.LogError(ee, "System Error while initializing Admin service. Message: ", ee.Message);
                bRetVal = false;
            }


            return bRetVal;
        }


        /// <summary>
        /// Api to get registry credentials using the service account credentials
        /// </summary>
        /// <param name="admin_system_key"></param>
        /// <param name="registry"></param>
        /// <returns>Success / Failure and RegistryKeyModel</returns>
        public async Task<(bool, RegistryKeyModel?)> GetRegistryCredentials(RegistryModel registry)
        {
            _logger.LogInformation("Getting registry credentials for registry {registry}.", registry.registry);

            try
            {
                // Initilize Base URL and auth token etc.
                if (!Initialize() || (_accountDetails == null) || (_api == null))
                    return (false, null);

                var response = await _api.GetRegistryCredentials(_accountDetails.systemKey, registry);
                if (response.IsSuccessStatusCode && response.Content != null)
                {
                    _logger.LogInformation("Successfully obtained registry credentials", response.Content);
                    return (true, response.Content);
                }
            }
            catch (Exception ee)
            {
                _logger.LogError(ee, "System Error while getting registry credentials. Message: ", ee.Message);
            }
            return (false, null);
        }
    }
}
