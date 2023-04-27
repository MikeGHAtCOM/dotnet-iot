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
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBlade.API.dotnet.client.core.Services
{
    internal interface IRegistryServiceContract
    {
        /// <summary>
        /// Api to get registry credentials using the service account credentials
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="parentPath"></param>
        /// <returns>RegistryKeyModel</returns>
        [Post("/api/v/{version}/webhook/execute/{system_key}/cloudiot")]
        Task<IApiResponse<RegistryConfigModel>> CreateRegistryAsync(int version, string system_key, [AliasAs("parent")] string parentPath, [Body] RegistryConfigModel registryIn);

        /// <summary>
        /// Api to get registry credentials using the service account credentials
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="parentPath"></param>
        /// <returns>Success / Failure</returns>
        [Delete("/api/v/{version}/webhook/execute/{system_key}/cloudiot")]
        Task<IApiResponse<bool>> DeleteRegistryAsync(int version, string system_key, [AliasAs("name")] string name);
    }
}
