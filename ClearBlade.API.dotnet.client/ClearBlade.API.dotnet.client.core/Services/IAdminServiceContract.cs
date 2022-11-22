using ClearBlade.API.dotnet.client.core.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBlade.API.dotnet.client.core.Services
{
    internal interface IAdminServiceContract
    {
        /// <summary>
        /// Api to get registry credentials using the service account credentials
        /// </summary>
        /// <param name="admin_system_key"></param>
        /// <param name="registry"></param>
        /// <returns>RegistryKeyModel</returns>
        [Post("/api/v/1/code/{admin_system_key}/getRegistryCredentials")]
        Task<IApiResponse<RegistryKeyModel>> GetRegistryCredentials(string admin_system_key, [Body] RegistryModel registry);
    }
}
