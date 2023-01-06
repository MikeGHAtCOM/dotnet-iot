using ClearBlade.API.dotnet.client.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBlade.API.dotnet.client.core.Services
{
    public interface IAdminService
    {
        /// <summary>
        /// Api to get registry credentials using the service account credentials
        /// </summary>
        /// <param name="admin_system_key"></param>
        /// <param name="registry"></param>
        /// <returns>Success / Failure and RegistryKeyModel</returns>
        Task<(bool, RegistryKeyModel?)> GetRegistryCredentials(RegistryModel registry);
    }
}
