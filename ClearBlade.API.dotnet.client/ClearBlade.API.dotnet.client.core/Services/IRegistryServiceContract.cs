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
        Task<IApiResponse<RegistryConfigModel>> CreateRegistry(int version, string system_key, [AliasAs("parent")] string parentPath, [Body] RegistryConfigModel registryIn);

        /// <summary>
        /// Api to get registry credentials using the service account credentials
        /// </summary>
        /// <param name="version"></param>
        /// <param name="system_key"></param>
        /// <param name="parentPath"></param>
        /// <returns>Success / Failure</returns>
        [Delete("/api/v/{version}/webhook/execute/{system_key}/cloudiot")]
        Task<IApiResponse<bool>> DeleteRegistry(int version, string system_key, [AliasAs("name")] string name);
    }
}
