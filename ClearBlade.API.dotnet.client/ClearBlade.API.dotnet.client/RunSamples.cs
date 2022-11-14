using ClearBlade.API.dotnet.client.core;
using ClearBlade.API.dotnet.client.core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBlade.API.dotnet.client
{
    /// <summary>
    /// Helper class to run and debug samples
    /// </summary>
    public static class RunSamples
    {
        #region Test Selection
        static bool bGetDevicesList = false;
        static bool bSendCommandToDevice = false;
        static bool bModifyCloudToDeviceConfig = false;
        static bool bCreateDevice = false;
        static bool bDeleteDevice = false;
        static bool bGetDevice = false;
        static bool bGetDeviceConfig = false;
        static bool bBindUnBindDevice = false;
        static bool bGetRegistryConfig= false;
        #endregion

        public static bool Execute(ServiceProvider serviceProvider, ILogger logger)
        {
            // Set which sample to run
            //bGetDevicesList = true;
            //bSendCommandToDevice = true;
            //bModifyCloudToDeviceConfig = true;
            //bCreateDevice = true;
            //bDeleteDevice = true;
            //bGetDeviceConfig = true;
            //bBindUnBindDevice = true;
            bGetRegistryConfig = true;


            logger.LogInformation("Running selected DotNet SDK samples");

            RunSamplesAsync(serviceProvider, logger).GetAwaiter().GetResult();

            logger.LogInformation("All done!");

            return true;
        }


        /// <summary>
        /// Method to run the tests asynchronously
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        static async Task RunSamplesAsync(ServiceProvider serviceProvider, ILogger logger)
        {
            IDeviceService? deviceService = serviceProvider.GetService<IDeviceService>();
            if (deviceService != null)
            {
                MainClient mClient = new MainClient(deviceService);

                // Sample - Obtain list of devices for a particular registry
                if (bGetDevicesList)
                {
                    // TBD - Need to obtain the token from authorization service
                    logger.LogInformation("Obtain list of devices for a particular registry");
                    var result = await mClient.GetDevicesList(4, "https://iot-sandbox.clearblade.com",
                                                                "92e9f2b60cd482c3b6e19984e48401",
                                                                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiI5Y2U5ZjJiNjBjODhlOWZjY2VkZGQ1YTZkZTBjIiwic2lkIjoiNjJmYzBlMTMtZWNkMy00OTUyLTgyN2UtOTI5YWJlODVkMTY2IiwidXQiOjIsInR0IjoxLCJleHAiOi0xLCJpYXQiOjE2NjgxNzY0NjJ9.M_ptSQZ6Y1qCzC8TszsbYo3Y8pjE56lQW9I4psin3JI",
                                                                "projects/ingressdevelopmentenv/locations/us-central1/registries/PD-103-Registry");
                    if (!result.Item1)
                        logger.LogInformation("Failed to get list of devices");
                    else
                    {
                        logger.LogInformation("Succeeded in getting the list of Devices");

                        // Use the list
                    }
                }

                // Sample - Send Command to Device
                if (bSendCommandToDevice)
                {
                    // TBD - Need to obtain the token from authorization service
                    logger.LogInformation("Send a Command to Device");

                    // Define the message you want to send
                    var data = new
                    {
                        binaryData = "QUJD"/*,
                        subfolder = "sub" - optional*/
                    };

                    var result = await mClient.SendCommandToDevice(4, "https://iot-sandbox.clearblade.com",
                                                                "92e9f2b60cd482c3b6e19984e48401",
                                                                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiI5Y2U5ZjJiNjBjODhlOWZjY2VkZGQ1YTZkZTBjIiwic2lkIjoiNjJmYzBlMTMtZWNkMy00OTUyLTgyN2UtOTI5YWJlODVkMTY2IiwidXQiOjIsInR0IjoxLCJleHAiOi0xLCJpYXQiOjE2NjgxNzY0NjJ9.M_ptSQZ6Y1qCzC8TszsbYo3Y8pjE56lQW9I4psin3JI",
                                                                "PD-103-Device", data);
                    if (!result)
                        logger.LogInformation("Failed to send command to device");
                    else
                    {
                        logger.LogInformation("Successfully sent command to device");
                    }
                }

                // Sample - Modify device configuration data
                if (bModifyCloudToDeviceConfig)
                {
                    // TBD - Need to obtain the token from authorization service
                    logger.LogInformation("Modify device configuration data");
                    var data = new
                    {
                        binaryData = "QUJD",
                        versionToUpdate = "19"
                    };
                    var result = await mClient.ModifyCloudToDeviceConfig(4, "https://iot-sandbox.clearblade.com",
                                                                "92e9f2b60cd482c3b6e19984e48401",
                                                                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiI5Y2U5ZjJiNjBjODhlOWZjY2VkZGQ1YTZkZTBjIiwic2lkIjoiNjJmYzBlMTMtZWNkMy00OTUyLTgyN2UtOTI5YWJlODVkMTY2IiwidXQiOjIsInR0IjoxLCJleHAiOi0xLCJpYXQiOjE2NjgxNzY0NjJ9.M_ptSQZ6Y1qCzC8TszsbYo3Y8pjE56lQW9I4psin3JI",
                                                                "PD-103-Device", data);
                    if (!result)
                        logger.LogInformation("Failed to modify the device config data");
                    else
                    {
                        logger.LogInformation("Successfully modified the device config data");
                    }
                }

                // Sample - Create new device
                if (bCreateDevice)
                {
                    // TBD - Need to obtain the token from authorization service

                    logger.LogInformation("Create a new device");

                    string id = "Sample-New-Device";
                    string name = "Sample-New-Device";

                    var result = await mClient.CreateDevice(4, "https://iot-sandbox.clearblade.com",
                                                                "92e9f2b60cd482c3b6e19984e48401",
                                                                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiI5Y2U5ZjJiNjBjODhlOWZjY2VkZGQ1YTZkZTBjIiwic2lkIjoiNjJmYzBlMTMtZWNkMy00OTUyLTgyN2UtOTI5YWJlODVkMTY2IiwidXQiOjIsInR0IjoxLCJleHAiOi0xLCJpYXQiOjE2NjgxNzY0NjJ9.M_ptSQZ6Y1qCzC8TszsbYo3Y8pjE56lQW9I4psin3JI",
                                                                id, name);
                    if (!result.Item1 || (result.Item2 == null))
                        logger.LogInformation("Failed to create new device");
                    else
                    {
                        logger.LogInformation("Successfully created new device");

                        // The result.Item2 object can be used to refer to newly created device
                    }
                }

                // Sample - Delete a device
                if (bDeleteDevice)
                {
                    // TBD - Need to obtain the token from authorization service
                    logger.LogInformation("Delete a device");

                    string id = "Sample-New-Device";
                    string name = "Sample-New-Device";

                    var result = await mClient.DeleteDevice(4, "https://iot-sandbox.clearblade.com",
                                                                "92e9f2b60cd482c3b6e19984e48401",
                                                                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiI5Y2U5ZjJiNjBjODhlOWZjY2VkZGQ1YTZkZTBjIiwic2lkIjoiNjJmYzBlMTMtZWNkMy00OTUyLTgyN2UtOTI5YWJlODVkMTY2IiwidXQiOjIsInR0IjoxLCJleHAiOi0xLCJpYXQiOjE2NjgxNzY0NjJ9.M_ptSQZ6Y1qCzC8TszsbYo3Y8pjE56lQW9I4psin3JI",
                                                                id, name);
                    if (!result.Item1 || (result.Item2 == null))
                        logger.LogInformation("Failed to delete device");
                    else
                    {
                        logger.LogInformation("Successfully deleted the device");
                    }
                }

                // Sample - Get a device
                if (bGetDevice)
                {
                    // TBD - Need to obtain the token from authorization service
                    logger.LogInformation("Get a device");

                    string name = "Sample-New-Device";

                    var result = await mClient.GetDevice(4, "https://iot-sandbox.clearblade.com",
                                                                "92e9f2b60cd482c3b6e19984e48401",
                                                                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiI5Y2U5ZjJiNjBjODhlOWZjY2VkZGQ1YTZkZTBjIiwic2lkIjoiNjJmYzBlMTMtZWNkMy00OTUyLTgyN2UtOTI5YWJlODVkMTY2IiwidXQiOjIsInR0IjoxLCJleHAiOi0xLCJpYXQiOjE2NjgxNzY0NjJ9.M_ptSQZ6Y1qCzC8TszsbYo3Y8pjE56lQW9I4psin3JI",
                                                                name);
                    if (!result.Item1 || (result.Item2 == null))
                        logger.LogInformation("Failed to get a device");
                    else
                    {
                        logger.LogInformation("Successfully obtained the device");
                    }
                }

                // Sample - Get configuration of a device
                if (bGetDeviceConfig)
                {
                    // TBD - Need to obtain the token from authorization service
                    logger.LogInformation("Get configuration of a device");

                    // While running this sample, it is assumed that, device with name
                    // "Sample-New-Device" exists and version is updated to "2"

                    string name = "Sample-New-Device";
                    string localVersion = "2";

                    var result = await mClient.GetDeviceConfig(4, "https://iot-sandbox.clearblade.com",
                                                                "92e9f2b60cd482c3b6e19984e48401",
                                                                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiI5Y2U5ZjJiNjBjODhlOWZjY2VkZGQ1YTZkZTBjIiwic2lkIjoiNjJmYzBlMTMtZWNkMy00OTUyLTgyN2UtOTI5YWJlODVkMTY2IiwidXQiOjIsInR0IjoxLCJleHAiOi0xLCJpYXQiOjE2NjgxNzY0NjJ9.M_ptSQZ6Y1qCzC8TszsbYo3Y8pjE56lQW9I4psin3JI",
                                                                name, localVersion);
                    if (!result.Item1 || (result.Item2 == null))
                        logger.LogInformation("Failed to get a device configuration");
                    else
                    {
                        logger.LogInformation("Successfully obtained the device configuration");

                        // Use the obtained information
                    }
                }

                // Sample - Bind / Unbind a device
                if (bBindUnBindDevice)
                {
                    // TBD - Need to obtain the token from authorization service
                    logger.LogInformation("Get configuration of a device");

                    // While running this sample, it is assumed that, device with name
                    // "Sample-New-Device" exists and Gateway with name "TestGateway" exists
                    // "Sample-New-Registry" is the registry name

                    // Sample - Bind Device
                    var result = await mClient.BindDeviceToGateway(4, "https://iot-sandbox.clearblade.com",
                                                                "c2d187b70cacfa9cc8bfe5d8e0e601",
                                                                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiJjYWQxODdiNzBjZDg5ZDgxZDZlMmMyZTllNzllMDEiLCJzaWQiOiIyNWQyOTQ4Mi0wZWQ0LTQzZGQtODQ1NS1mZmYzOTYxOTg1ODciLCJ1dCI6MiwidHQiOjEsImV4cCI6LTEsImlhdCI6MTY2ODM0Njk4MX0.gN7DR4ri-shVaY_wKvYQUC-R6NIP1oy_CC0a88vjBDU",
                                                                "Sample-New-Registry", "TestGateway", "Sample-New-Device");
                    if (!result)
                    {
                        logger.LogInformation("Failed To Bind Device");
                    }
                    else
                    {
                        // Actual test - UnBind Device
                        result = await mClient.UnBindDeviceFromGateway(4, "https://iot-sandbox.clearblade.com",
                                                                    "c2d187b70cacfa9cc8bfe5d8e0e601",
                                                                    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiJjYWQxODdiNzBjZDg5ZDgxZDZlMmMyZTllNzllMDEiLCJzaWQiOiIyNWQyOTQ4Mi0wZWQ0LTQzZGQtODQ1NS1mZmYzOTYxOTg1ODciLCJ1dCI6MiwidHQiOjEsImV4cCI6LTEsImlhdCI6MTY2ODM0Njk4MX0.gN7DR4ri-shVaY_wKvYQUC-R6NIP1oy_CC0a88vjBDU",
                                                                    "Sample-New-Registry", "TestGateway", "Sample-New-Device");
                        if (!result)
                            logger.LogInformation("Failed to unbind a device");
                        else
                            logger.LogInformation("Successfully bind device");

                    }
                }

                // Sample - Get configuration of a device
                if (bGetRegistryConfig)
                {
                    // TBD - Need to obtain the token from authorization service
                    logger.LogInformation("Get configuration of a registry");

                    // While running this sample, it is assumed that, registry with name
                    // "Sample-New-Registry" exists and version is updated to "2"

                    string name = "Sample-New-Registry";

                    var result = await mClient.GetRegistryConfig(4, "https://iot-sandbox.clearblade.com",
                                                                "c2d187b70cacfa9cc8bfe5d8e0e601",
                                                                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiJjYWQxODdiNzBjZDg5ZDgxZDZlMmMyZTllNzllMDEiLCJzaWQiOiIyNWQyOTQ4Mi0wZWQ0LTQzZGQtODQ1NS1mZmYzOTYxOTg1ODciLCJ1dCI6MiwidHQiOjEsImV4cCI6LTEsImlhdCI6MTY2ODM0Njk4MX0.gN7DR4ri-shVaY_wKvYQUC-R6NIP1oy_CC0a88vjBDU",
                                                                name);
                    if (!result.Item1 || (result.Item2 == null))
                        logger.LogInformation("Failed to get a device configuration");
                    else
                    {
                        logger.LogInformation("Successfully obtained the device configuration");

                        // Use the obtained information
                    }
                }

                //Console.ReadLine();
            }
        }

    }
}
