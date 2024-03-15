/*
 * Copyright (c) 2023 ClearBlade Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 *
 * Copyright (c) 2018 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 */
 
using ClearBlade.API.dotnet.client.core.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Refit;
using System.Net.Http;

namespace ClearBlade.API.dotnet.client.core.Services
{
    public class RegistryService : IRegistryService
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
                var configDetails = System.Environment.GetEnvironmentVariable("CLEARBLADE_CONFIGASVARIABLE", EnvironmentVariableTarget.Process);
                if (string.IsNullOrEmpty(configDetails)) 
                {
                    _accountDetails = JsonConvert.DeserializeObject<ServiceAccountDetails>(configDetails);
                }

                if (_accountDetails == null)
                {
                    // First get the location of Service account private key json
                    // using windows environment (system) variable named "CLEARBLADE_CONFIGURATION"
                    var jsonPath = System.Environment.GetEnvironmentVariable("CLEARBLADE_CONFIGURATION", EnvironmentVariableTarget.Process);
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
                }

#pragma warning disable IDE0090 // Use 'new(...)'
                HttpLoggingHandler handler = new HttpLoggingHandler(_accountDetails.Token);
#pragma warning restore IDE0090 // Use 'new(...)'
                _api = RestService.For<IRegistryServiceContract>(new HttpClient(handler)
                {
                    BaseAddress = new Uri(_accountDetails.Url)
#if NET472
                }, new RefitSettings(new NewtonsoftJsonContentSerializer()));
#else
                });
#endif
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
        public async Task<(bool, RegistryConfigModel?)> CreateRegistryAsync(int version, string parentPath, RegistryConfigModel registryConfigModel)
        {
            _logger.LogInformation("Creating registry {registryConfigModel}.", registryConfigModel);

            try
            {
                // Initilize Base URL and auth token etc.
                if (!Initialize() || (_accountDetails == null) || (_api == null))
                    return (false, null);

                var response = await _api.CreateRegistryAsync(version, _accountDetails.SystemKey, parentPath, registryConfigModel);
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
        public async Task<bool> DeleteRegistryAsync(int version, string registryName)
        {
            _logger.LogInformation("Deleting registry {registryName}.", registryName);

            try
            {
                // Initilize Base URL and auth token etc.
                if (!Initialize() || (_accountDetails == null) || (_api == null))
                    return false;

                var response = await _api.DeleteRegistryAsync(version, _accountDetails.SystemKey, registryName);
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
