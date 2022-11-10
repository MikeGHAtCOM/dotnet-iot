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
        #endregion

        public static bool Execute(ServiceProvider serviceProvider, ILogger logger)
        {
            // Set which sample to run
            //bGetDevicesList = true;
            //bSendCommandToDevice = true;
            //bModifyCloudToDeviceConfig = true;
            //bCreateDevice = true;
            //bDeleteDevice = true;
            bGetDevice = true;


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
                                                                "f6e1d8b30cb0cd8fe8cf95d0dfd001",
                                                                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiJmZWUxZDhiMzBjODY5MmRiOTZjMGNiODllYzNmIiwic2lkIjoiNjg2MDExZGYtM2VhZS00NjYxLWFlNDYtMGUzNDk4NTBjYzdiIiwidXQiOjIsInR0IjoxLCJleHAiOi0xLCJpYXQiOjE2NjQ4MTcyNzl9.PuzZnogOYym0U7k130oTVqnNwt7RvVGq6G8JZ0SRrss",
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
                                                                "f6e1d8b30cb0cd8fe8cf95d0dfd001",
                                                                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiJmZWUxZDhiMzBjODY5MmRiOTZjMGNiODllYzNmIiwic2lkIjoiNjg2MDExZGYtM2VhZS00NjYxLWFlNDYtMGUzNDk4NTBjYzdiIiwidXQiOjIsInR0IjoxLCJleHAiOi0xLCJpYXQiOjE2NjQ4MTcyNzl9.PuzZnogOYym0U7k130oTVqnNwt7RvVGq6G8JZ0SRrss",
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
                                                                "f6e1d8b30cb0cd8fe8cf95d0dfd001",
                                                                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiJmZWUxZDhiMzBjODY5MmRiOTZjMGNiODllYzNmIiwic2lkIjoiNjg2MDExZGYtM2VhZS00NjYxLWFlNDYtMGUzNDk4NTBjYzdiIiwidXQiOjIsInR0IjoxLCJleHAiOi0xLCJpYXQiOjE2NjQ4MTcyNzl9.PuzZnogOYym0U7k130oTVqnNwt7RvVGq6G8JZ0SRrss",
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
                                                                "f6e1d8b30cb0cd8fe8cf95d0dfd001",
                                                                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiJmZWUxZDhiMzBjODY5MmRiOTZjMGNiODllYzNmIiwic2lkIjoiNjg2MDExZGYtM2VhZS00NjYxLWFlNDYtMGUzNDk4NTBjYzdiIiwidXQiOjIsInR0IjoxLCJleHAiOi0xLCJpYXQiOjE2NjQ4MTcyNzl9.PuzZnogOYym0U7k130oTVqnNwt7RvVGq6G8JZ0SRrss",
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
                                                                "f6e1d8b30cb0cd8fe8cf95d0dfd001",
                                                                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiJmZWUxZDhiMzBjODY5MmRiOTZjMGNiODllYzNmIiwic2lkIjoiNjg2MDExZGYtM2VhZS00NjYxLWFlNDYtMGUzNDk4NTBjYzdiIiwidXQiOjIsInR0IjoxLCJleHAiOi0xLCJpYXQiOjE2NjQ4MTcyNzl9.PuzZnogOYym0U7k130oTVqnNwt7RvVGq6G8JZ0SRrss",
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
                                                                "f6e1d8b30cb0cd8fe8cf95d0dfd001",
                                                                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiJmZWUxZDhiMzBjODY5MmRiOTZjMGNiODllYzNmIiwic2lkIjoiNjg2MDExZGYtM2VhZS00NjYxLWFlNDYtMGUzNDk4NTBjYzdiIiwidXQiOjIsInR0IjoxLCJleHAiOi0xLCJpYXQiOjE2NjQ4MTcyNzl9.PuzZnogOYym0U7k130oTVqnNwt7RvVGq6G8JZ0SRrss",
                                                                name);
                    if (!result.Item1 || (result.Item2 == null))
                        logger.LogInformation("Failed to get a device");
                    else
                    {
                        logger.LogInformation("Successfully obtained the device");
                    }
                }


                //Console.ReadLine();
            }
        }

    }
}
