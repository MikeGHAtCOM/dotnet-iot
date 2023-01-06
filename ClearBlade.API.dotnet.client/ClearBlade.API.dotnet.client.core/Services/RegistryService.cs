using ClearBlade.API.dotnet.client.core.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Refit;

namespace ClearBlade.API.dotnet.client.core.Services
{
    internal class RegistryService : IRegistryService
    {
        private readonly ILogger<RegistryService> _logger;
        private IRegistryServiceContract? _api;
        private ServiceAccountDetails? _accountDetails;

        public RegistryService(ILogger<RegistryService> logger)
        {
            _logger = logger;
            _api = null;
        }

        /// <summary>
        /// Method used to initialize the registry service. This essentially provides
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

                HttpLoggingHandler handler = new HttpLoggingHandler(_accountDetails.Token);
                string baseUrl = _accountDetails.Url;
                _api = RestService.For<IRegistryServiceContract>(new HttpClient(handler)
                {
                    BaseAddress = new Uri(_accountDetails.Url)
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
        /// Api to create a registry
        /// </summary>
        /// <param name="registry"></param>
        /// <returns>Success / Failure</returns>
        public async Task<(bool, RegistryConfigModel?)> CreateRegistry(int version, string parentPath, RegistryConfigModel registryConfigModel)
        {
            _logger.LogInformation("Creating registry {registryConfigModel}.", registryConfigModel);

            try
            {
                // Initilize Base URL and auth token etc.
                if (!Initialize() || (_accountDetails == null) || (_api == null))
                    return (false, null);

                var response = await _api.CreateRegistry(version, _accountDetails.SystemKey, parentPath, registryConfigModel);
                if (response.IsSuccessStatusCode && response.Content != null)
                {
                    _logger.LogInformation("Successfully created registry", response.Content);
                    return (true, response.Content);
                }
            }
            catch (Exception ee)
            {
                _logger.LogError(ee, "System Error while creating registry. Message: ", ee.Message);
            }
            return (false, null);
        }

        /// <summary>
        /// Api to delete a registry
        /// </summary>
        /// <param name="version"></param>
        /// <param name="registryName"></param>
        /// <returns>Success / Failure</returns>
        public async Task<bool> DeleteRegistry(int version, string registryName)
        {
            _logger.LogInformation("Deleting registry {registryName}.", registryName);

            try
            {
                // Initilize Base URL and auth token etc.
                if (!Initialize() || (_accountDetails == null) || (_api == null))
                    return false;

                var response = await _api.DeleteRegistry(version, _accountDetails.SystemKey, registryName);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully deleted registry");
                    return true;
                }
            }
            catch (Exception ee)
            {
                _logger.LogError(ee, "System Error while deleting registry. Message: ", ee.Message);
            }
            return false;
        }
    }
}
