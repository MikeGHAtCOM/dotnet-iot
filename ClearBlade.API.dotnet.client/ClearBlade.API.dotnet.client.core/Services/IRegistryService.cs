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
        Task<(bool, RegistryConfigModel?)> CreateRegistry(int version, string parentPath, RegistryConfigModel registryConfigModel);

        /// <summary>
        /// Api to delete a registry
        /// </summary>
        /// <param name="version"></param>
        /// <returns>Success / Failure</returns>
        Task<bool> DeleteRegistry(int version, string registryName);
    }
}
