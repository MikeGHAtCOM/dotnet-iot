﻿using ClearBlade.API.dotnet.client.core;
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
            //bGetRegistryConfig = true;


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
                    logger.LogInformation("Obtain list of devices for a particular registry");
                    var result = await mClient.GetDevicesList(4, "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry");
                    if (!result.Item1)
                        logger.LogError("Failed to get list of devices");
                    else
                    {
                        logger.LogInformation("Succeeded in getting the list of Devices");

                        // Use the list
                    }
                }

                // Sample - Send Command to Device
                if (bSendCommandToDevice)
                {
                    logger.LogInformation("Send a Command to Device");

                    // Define the message you want to send
                    var data = new
                    {
                        binaryData = "QUJD"/*,
                        subfolder = "sub" - optional*/
                    };

                    var result = await mClient.SendCommandToDevice(4, "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Sample-New-Device", data);
                    if (!result)
                        logger.LogError("Failed to send command to device");
                    else
                    {
                        logger.LogInformation("Successfully sent command to device");
                    }
                }

                // Sample - Modify device configuration data
                if (bModifyCloudToDeviceConfig)
                {
                    logger.LogInformation("Modify device configuration data");
                    var data = new
                    {
                        binaryData = "QUJD",
                        versionToUpdate = "19"
                    };
                    var result = await mClient.ModifyCloudToDeviceConfig(4, "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Sample-New-Device", data);
                    if (!result)
                        logger.LogError("Failed to modify the device config data");
                    else
                    {
                        logger.LogInformation("Successfully modified the device config data");
                    }
                }

                // Sample - Create new device
                if (bCreateDevice)
                {
                    logger.LogInformation("Create a new device");

                    string id = "Sample-New-Device";
                    string name = "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Sample-New-Device";

                    var result = await mClient.CreateDevice(4, id, name);
                    if (!result.Item1 || (result.Item2 == null))
                        logger.LogError("Failed to create new device");
                    else
                    {
                        logger.LogInformation("Successfully created new device");

                        // The result.Item2 object can be used to refer to newly created device
                    }
                }

                // Sample - Delete a device
                if (bDeleteDevice)
                {
                    logger.LogInformation("Delete a device");

                    string id = "Sample-New-Device";
                    string name = "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Sample-New-Device";

                    var result = await mClient.DeleteDevice(4, id, name);
                    if (!result.Item1 || (result.Item2 == null))
                        logger.LogError("Failed to delete device");
                    else
                    {
                        logger.LogInformation("Successfully deleted the device");
                    }
                }

                // Sample - Get a device
                if (bGetDevice)
                {
                    logger.LogInformation("Get a device");

                    string name = "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Sample-New-Device";

                    var result = await mClient.GetDevice(4, name);
                    if (!result.Item1 || (result.Item2 == null))
                        logger.LogError("Failed to get a device");
                    else
                    {
                        logger.LogInformation("Successfully obtained the device");
                    }
                }

                // Sample - Get configuration of a device
                if (bGetDeviceConfig)
                {
                    logger.LogInformation("Get configuration of a device");

                    // While running this sample, it is assumed that, device with name
                    // "Sample-New-Device" exists and version is updated to "2"

                    string name = "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Sample-New-Device";
                    string localVersion = "2";

                    var result = await mClient.GetDeviceConfig(4, name, localVersion);
                    if (!result.Item1 || (result.Item2 == null))
                        logger.LogError("Failed to get a device configuration");
                    else
                    {
                        logger.LogInformation("Successfully obtained the device configuration");

                        // Use the obtained information
                    }
                }

                // Sample - Bind / Unbind a device
                if (bBindUnBindDevice)
                {
                    logger.LogInformation("Get configuration of a device");

                    // While running this sample, it is assumed that, device with name
                    // "Sample-New-Device" exists and Gateway with name "TestGateway" exists
                    // "Sample-New-Registry" is the registry name

                    // Sample - Bind Device
                    var result = await mClient.BindDeviceToGateway(4, "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry", "TestGateway", "Sample-New-Device");
                    if (!result)
                    {
                        logger.LogError("Failed To Bind Device");
                    }
                    else
                    {
                        // Actual test - UnBind Device
                        result = await mClient.UnBindDeviceFromGateway(4, "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry", "TestGateway", "Sample-New-Device");
                        if (!result)
                            logger.LogError("Failed to unbind a device");
                        else
                            logger.LogInformation("Successfully bind device");

                    }
                }

                // Sample - Get configuration of a device
                if (bGetRegistryConfig)
                {
                    logger.LogInformation("Get configuration of a registry");

                    // While running this sample, it is assumed that, registry with name
                    // "Sample-New-Registry" exists and version is updated to "2"

                    string name = "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry";

                    var result = await mClient.GetRegistryConfig(4, name);
                    if (!result.Item1 || (result.Item2 == null))
                        logger.LogError("Failed to get a device configuration");
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
