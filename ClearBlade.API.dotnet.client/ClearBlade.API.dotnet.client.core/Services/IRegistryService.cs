﻿/*
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

namespace ClearBlade.API.dotnet.client.core.Services
{
    public interface IRegistryService
    {
        /// <summary>
        /// Method used to initialize the registry service. This essentially provides
        /// the base URL of the ClearBlade regional IOT and a handler that contains the
        /// authorization token
        /// </summary>
        bool Initialize();

        /// <summary>
        /// Api to create a registry
        /// </summary>
        /// <param name="version"></param>
        /// <param name="parentPath"></param>
        /// <param name="registryConfigModel"></param>
        /// <returns>RegistryConfigModel</returns>
        Task<(bool, RegistryConfigModel?)> CreateRegistryAsync(int version, string parentPath, RegistryConfigModel registryConfigModel);

        /// <summary>
        /// Api to delete a registry
        /// </summary>
        /// <param name="version"></param>
        /// <returns>Success / Failure</returns>
        Task<bool> DeleteRegistryAsync(int version, string registryName);
    }
}
