using ClearBlade.API.dotnet.client.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBlade.API.dotnet.client.core.Services
{
    public interface IRegistryService
    {

        /// <summary>
        /// Api to create a registry
        /// </summary>
        /// <param name="version"></param>
        /// <param name="parentPath"></param>
        /// <param name="registryConfigModel"></param>
        /// <returns>RegistryConfigModel</returns>
        Task<(bool, RegistryConfigModel?)> CreateRegistry(int version, string parentPath, RegistryConfigModel registryConfigModel);

        /// <summary>
        /// Api to delete a registry
        /// </summary>
        /// <param name="version"></param>
        /// <returns>Success / Failure</returns>
        Task<bool> DeleteRegistry(int version, string registryName);
    }
}
