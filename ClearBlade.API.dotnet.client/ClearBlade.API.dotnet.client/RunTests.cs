using ClearBlade.API.dotnet.client.core;
using ClearBlade.API.dotnet.client.core.Models;
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
    /// Helper class to run tests to test the dot net SDK
    /// </summary>
    public static class RunTests
    {
        #region Test Selection
        static bool bAllTests = false;
        static bool bTest001 = false;
        static bool bTest002 = false;
        static bool bTest003 = false;
        static bool bTest004 = false;
        static bool bTest005 = false;
        static bool bTest006 = false;
        static bool bTest007 = false;
        static bool bTest008 = false;
        static bool bTest009 = false;
        static bool bTest010 = false;
        static bool bTest011 = false;
        static bool bTest012 = false;
        static bool bTest013 = false;
        #endregion

        public static bool Execute(ServiceProvider serviceProvider, ILogger logger)
        {
            // Set which tests to run
            // bTest013 = true;
            // bTest008 = true;
             //bTest007 = true;
            // bTest004 = true;
            // bTest005 = true;
            // bTest006 = true;
            //bTest013 = true;
             bAllTests = true;

            logger.LogInformation("Running selected DotNet SDK tests");

            RunTestsAsync(serviceProvider, logger).GetAwaiter().GetResult();

            logger.LogInformation("All done!");

            return true;
        }


        /// <summary>
        /// Method to run the tests asynchronously
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        static async Task RunTestsAsync(ServiceProvider serviceProvider, ILogger logger)
        {
            IDeviceService? deviceService = serviceProvider.GetService<IDeviceService>();
            if (deviceService != null)
            {
                MainClient mClient = new MainClient(deviceService);

                // Test-001 - Obtain list of devices for a particular registry
                if (bTest001 || bAllTests)
                {
                    logger.LogInformation("Running Test-001 - Obtain list of devices for a particular registry");

                    // Create a device to verify if result is correct
                    var resultPre = await mClient.CreateDevice(4, "Test-001-Device", "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-001-Device");
                    if (!resultPre.Item1 || (resultPre.Item2 == null))
                    {
                        logger.LogError("Test-001 - Create Device - Failed");
                    }
                    else
                    {

                        var result = await mClient.GetDevicesList(4, "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry");
                        if (!result.Item1)
                            logger.LogError("Test-001 - Failed");
                        else
                        {
                            bool bSuccess = false;                            

                            foreach (var deviceItem in result.Item2)
                            {
                                if(string.Compare(deviceItem.Name, "Test-001-Device", true) == 0)
                                {
                                    bSuccess = true;
                                    break;
                                }
                            }
                            if (bSuccess)
                                logger.LogInformation("Test-001 - Succeeded");
                            else
                                logger.LogError("Test-001 - Failed");
                        }

                        // Delete the newly created device - cleanup
                        await mClient.DeleteDevice(4, "Test-001-Device", "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-001-Device");
                    }
                }

                // Test-002 - Send Command to Device
                if (bTest002 || bAllTests)
                {
                    logger.LogInformation("Running Test-002 - Send Command to Device");

                    var data = new
                    {
                        binaryData = "QUJD",
                        versionToUpdate = "1"
                    };

                    // Create new device to send command to
                    var resultPre = await mClient.CreateDevice(4, "Test-002-Device", "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Test-002-Device");
                    if (!resultPre.Item1 || (resultPre.Item2 == null))
                        logger.LogError("Test-002 - Failed");

                    // Next bind the device to gateway
                    var result008 = await mClient.BindDeviceToGateway(4, "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry", "TestGateway", "Test-002-Device");
                    if (!result008)
                        logger.LogError("Test-002 - Failed");

                    // Now send the message to newly create device
                    var result002 = await mClient.SendCommandToDevice(4, "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-002-Device", data);
                    if (!result002)
                        logger.LogError("Test-002 - Failed");
                    else
                    {
                        logger.LogInformation("Test-002 - Succeeded");
                    }

                    // Delete the newly created device - cleanup
                    await mClient.DeleteDevice(4, "Test-002-Device", "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-002-Device");
                }

                // Test-003 - Modify Device config
                if (bTest003 || bAllTests)
                {
                    // Create new device to send command to
                    var resultPre = await mClient.CreateDevice(4, "Test-003-Device", "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Test-003-Device");
                    if (!resultPre.Item1 || (resultPre.Item2 == null))
                        logger.LogError("Test-003 - Failed");
                    
                    logger.LogInformation("Running Test-003 - Modify device config");
                    var data = new
                    {
                        binaryData = "QUJD",
                        versionToUpdate = "1"
                    };
                    var result003 = await mClient.ModifyCloudToDeviceConfig(4, "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-003-Device", data);
                    if (!result003)
                        logger.LogError("Test-003 - Failed");
                    else
                    {
                        logger.LogInformation("Test-003 - Succeeded");
                    }
                    // Delete the newly created device - cleanup
                    await mClient.DeleteDevice(4, "Test-003-Device", "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-003-Device");
                }

                // Test-004 - Create Device
                if (bTest004 || bAllTests)
                {
                    logger.LogInformation("Running Test-004 - Create Device");

                    // Delete the device with ID "Test-004-Device" if it already existed.
                    await mClient.DeleteDevice(4, "Test-004-Device", "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-004-Device");

                    // Create new device
                    var result004 = await mClient.CreateDevice(4, "Test-004-Device", "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-004-Device");
                    if (!result004.Item1 || (result004.Item2 == null))
                        logger.LogError("Test-004 - Failed");
                    else
                    {
                        // Verify if the device exists
                        var result = await mClient.GetDevicesList(4, "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry");
                        if (!result.Item1)
                            logger.LogError("Test-004 - Failed");
                        else
                        {
                            bool bSuccess = false;

                            foreach (var deviceItem in result.Item2)
                            {
                                if (string.Compare(deviceItem.Name, "Test-004-Device", true) == 0)
                                {
                                    bSuccess = true;
                                    break;
                                }
                            }
                            if (bSuccess)
                            {
                                logger.LogInformation("Test-004 - Succeeded");

                                // Delete the newly created device - cleanup
                                await mClient.DeleteDevice(4, "Test-004-Device", "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-004-Device");
                            }
                            else
                            {
                                logger.LogError("Test-004 - Failed");
                            }
                        }
                    }
                }

                // Test-005 - Delete Device
                if (bTest005 || bAllTests)
                {
                    logger.LogInformation("Running Test-005 - Delete Device");

                    // First create a device to delete it
                    var resultPre = await mClient.CreateDevice(4, "Test-005-Device", "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-005-Device");
                    if (!resultPre.Item1 || (resultPre.Item2 == null))
                        logger.LogError("Test-005 - Failed");
                    else
                    {

                        var result005 = await mClient.DeleteDevice(4, "Test-005-Device", "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-005-Device");
                        if (!result005.Item1 || (result005.Item2 == null))
                            logger.LogError("Test-005 - Failed");
                        else
                        {
                            // try to get the device
                            var resultPost = await mClient.GetDevice(4, "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-005-Device");
                            if (!resultPost.Item1 || (resultPost.Item2 == null))
                                logger.LogInformation("Test-005 - Succeeded"); // Device does not exist means it is deleted
                            else
                                logger.LogError("Test-005 - Failed"); // Device still exists. Means test case failed.
                        }
                    }
                }

                // Test-006 - Get Device details
                if (bTest006 || bAllTests)
                {
                    logger.LogInformation("Running Test-006 - Get Device");

                    // First create a device to get its details
                    var resultPre = await mClient.CreateDevice(4, "Test-006-Device", "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-006-Device");
                    if (!resultPre.Item1 || (resultPre.Item2 == null))
                        logger.LogError("Test-006 - Failed");

                    var result006 = await mClient.GetDevice(4, "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-006-Device");
                    if (!result006.Item1 || (result006.Item2 == null))
                        logger.LogError("Test-006 - Failed");
                    else
                    {
                        if(string.Compare(result006.Item2.Name, "Test-006-Device", true) == 0)
                            logger.LogInformation("Test-006 - Succeeded");
                        else
                            logger.LogError("Test-006 - Failed");

                        // Delete the newly created device - cleanup
                        await mClient.DeleteDevice(4, "Test-006-Device", "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-006-Device");

                    }
                }

                // Test-007 - Create Device configuration details
                if (bTest007 || bAllTests)
                {
                    logger.LogInformation("Running Test-007 - Get Device");

                    // First create a device to get its configuration details
                    var resultPre = await mClient.CreateDevice(4, "Test-007-Device", "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-007-Device");
                    if (!resultPre.Item1 || (resultPre.Item2 == null))
                        logger.LogError("Test-007 - Failed - Failed while creating new device");

                    // Next set some configuration information
                    var data = new
                    {
                        binaryData = "QUJD",
                        versionToUpdate = "1"
                    };
                    var resultPre1 = await mClient.ModifyCloudToDeviceConfig(4, "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-007-Device", data);
                    if (!resultPre1)
                        logger.LogError("Test-007 - Failed - Failed while setting configuration");
                    
                    // Actual test
                    var result007 = await mClient.GetDeviceConfig(4, "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-007-Device", "2");
                    if (!result007.Item1 || (result007.Item2 == null))
                        logger.LogError("Test-007 - Failed");
                    else
                    {
                        if (string.Compare(result007.Item2.BinaryData, "QUJD", true) == 0)
                            logger.LogInformation("Test-007 - Succeeded");
                        else
                            logger.LogError("Test-007 - Failed");
                    }
                    // Delete the newly created device - cleanup
                    await mClient.DeleteDevice(4, "Test-007-Device", "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-007-Device");
                }

                // Test-008 - Get Device configuration details
                if (bTest008 || bAllTests)
                {
                    logger.LogInformation("Running Test-008 - Get Device");

                    // First create a device to get its configuration details
                    var resultPre = await mClient.CreateDevice(4, "Test-008-Device", "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-008-Device");
                    if (!resultPre.Item1 || (resultPre.Item2 == null))
                        logger.LogError("Test-008 - Failed - Failed while creating new device");

                   
                    // Actual test - Bind Device
                    var result008 = await mClient.BindDeviceToGateway(4, "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry", "TestGateway", "Test-008-Device");
                    if (!result008)
                        logger.LogError("Test-008 - Failed");
                    else
                    {
                        // Actual test - UnBind Device
                        result008 = await mClient.UnBindDeviceFromGateway(4, "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry", "TestGateway", "Test-008-Device");
                        if (!result008)
                            logger.LogError("Test-008 - Failed");
                        else
                            logger.LogInformation("Test-008 - Succeeded");

                        // Delete the newly created device - cleanup
                        await mClient.DeleteDevice(4, "Test-008-Device", "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-008-Device");
                    }
                }

                // Test-009 - Get Registry configuration details
                if (bTest009 || bAllTests)
                {
                    logger.LogInformation("Running Test-009 - Get Registry configuration");

                    // Get configuration information
                    var result009 = await mClient.GetRegistryConfig(4, "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry");
                    if (!result009.Item1 || (result009.Item2 == null))
                        logger.LogError("Test-009 - Failed");
                    else
                    {
                        if (string.Compare(result009.Item2.Id, "Sample-New-Registry", true) == 0)
                            logger.LogInformation("Test-009 - Succeeded");
                        else
                            logger.LogError("Test-009 - Failed");
                    }
                }

                // Test-010 - Patch Registry configuration details
                if (bTest010 || bAllTests)
                {
                    logger.LogInformation("Running Test-010 - patch Registry configuration");

                    string regName = "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry";

                    // First get some configuration information
                    var resultPre = await mClient.GetRegistryConfig(4, regName);
                    if (!resultPre.Item1 || (resultPre.Item2 == null))
                        logger.LogError("Test-010 - Failed to find Registry configuration");
                    else
                    {
                        string updateMask = "httpConfig.http_enabled_state,mqttConfig.mqtt_enabled_state";
                        resultPre.Item2.MqttConfig.MqttEnabledState = "MQTT_ENABLED";
                        resultPre.Item2.HttpConfig.HttpEnabledState = "HTTP_ENABLED";

                        var result010 = await mClient.PatchRegistry(4, regName, updateMask, resultPre.Item2);
                        if (!result010.Item1 || (result010.Item2 == null))
                            logger.LogError("Test-010 - Failed to update Registry configuration");
                        else
                        {
                            if ((string.Compare(result010.Item2.Id, "Sample-New-Registry", true) == 0) &&
                                (string.Compare(result010.Item2.MqttConfig.MqttEnabledState, "MQTT_ENABLED", true) == 0) &&
                                (string.Compare(result010.Item2.HttpConfig.HttpEnabledState, "HTTP_ENABLED", true) == 0))
                                logger.LogInformation("Test-010 - Succeeded");
                            else
                                logger.LogError("Test-010 - Failed");
                        }
                    }
                }

                // Test-011 - Patch Device configuration details
                if (bTest011 || bAllTests)
                {
                    logger.LogInformation("Running Test-011 - patch Device configuration");

                    string deviceName = "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Sample-New-Device";

                    // First get some configuration information
                    var resultPre = await mClient.GetDevice(4, deviceName);
                    if (!resultPre.Item1 || (resultPre.Item2 == null))
                        logger.LogError("Test-011 - Failed to find Device configuration");
                    else
                    {
                        string updateMask = "metadata";
                        string pubKey = "-----BEGIN PUBLIC KEY-----\nMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA5P0Z4OUD5PSjri8xexGo\n6eQ39NGyQbXamIgWAwvnAs/oDRVqEejE2nwDhnpykaCGLkuDEN0LPd2wF+vC2Cq3\nY3YvkJh71IkjuAjMZQ+00CXdezfCjmTtEpMCNA3cV+G1g6uIcdEpHKs0YHfC9CFQ\nrjkc7tl3idmcQLngIov/gsFY7D1pbOgkCVVcZCRLgsdFfhCUYwYCvdEVJP3w+5mG\nybvmhNRbbFG7eG3+hmZoOg0h3f6r2fqgSx6l0+Z3D77SRT6lBEHvGDlxb08ASeuE\n0SJAc6PdAKd3FDqdZok4z1qJsgMqtU/ZGJJG54pNECWmhoOar+aQmmqnZ6kGQ5cn\nEwIDAQAB\n-----END PUBLIC KEY-----\n";
                        resultPre.Item2.Credentials.Add(new core.Models.Credential
                        {
                            ExpirationTime = "",
                            PublicKey = new PublicKey
                            {
                                format = "RSA_PEM",
                                key = pubKey
                            }
                        });

                        var result011 = await mClient.PatchDevice(4, deviceName, updateMask, resultPre.Item2);
                        if (!result011.Item1 || (result011.Item2 == null))
                            logger.LogError("Test-011 - Failed to update Registry configuration");
                        else
                        {
                            string resPubKey = string.Empty;
                            if ((result011.Item2 != null) && result011.Item2.Credentials.Count > 0 && result011.Item2.Credentials.FirstOrDefault() != null)
                            {
                                foreach (var item in result011.Item2.Credentials)
                                {
                                    resPubKey = item.PublicKey.key;
                                }
                            }
                            if ((string.Compare(result011.Item2?.Id, "Sample-New-Device", true) == 0) &&
                            (string.Compare(resPubKey, pubKey, true) == 0))
                                logger.LogInformation("Test-011 - Succeeded");
                            else
                                logger.LogError("Test-011 - Failed");
                        }
                    }
                }

                // Test-012 - Get Device configuration versions list
                if (bTest012 || bAllTests)
                {
                    logger.LogInformation("Running Test-012 - get Device configuration versions list");

                    string deviceName = "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-012-Device";

                    // First create a device to add config versions
                    var resultPre = await mClient.CreateDevice(4, "Test-012-Device", deviceName);
                    if (!resultPre.Item1 || (resultPre.Item2 == null))
                        logger.LogError("Test-012 - Failed");

                    // Next set some configuration information
                    var data = new
                    {
                        binaryData = "QUJD",
                        versionToUpdate = "1"
                    };
                    var resultPre1 = await mClient.ModifyCloudToDeviceConfig(4, deviceName, data);
                    if (!resultPre1)
                        logger.LogError("Test-012 - Failed - Failed while setting configuration");

                    // Actual test
                    var result012 = await mClient.GetDeviceConfigVersionList(4, deviceName, 5);
                    if (!result012.Item1 || (result012.Item2 == null))
                        logger.LogError("Test-012 - Failed to get Device configuration list");
                    else
                    {
                        // Verify obtained data
                        if(result012.Item2.deviceConfigs.Count != 2)
                            logger.LogError("Test-012 - Failed to get Device configuration list");
                        else
                        {
                            var firstConfig = result012.Item2.deviceConfigs.FirstOrDefault();
                            if((firstConfig == null) || (firstConfig.Version != "2")) // latest will be first
                                logger.LogError("Test-012 - Failed to get Device configuration list");
                            else
                            {
                                var secondConfig = result012.Item2.deviceConfigs.LastOrDefault();
                                if ((secondConfig == null) || (secondConfig.Version != "1")) // older will be next
                                    logger.LogError("Test-012 - Failed to get Device configuration list");
                                else
                                    logger.LogInformation("Test-012 - Succeeded");
                            }
                        }                        
                    }
                    // Delete the newly created device - cleanup
                    await mClient.DeleteDevice(4, "Test-012-Device", deviceName);
                }

                // Test-013 - Get Device states list
                if (bTest013 || bAllTests)
                {
                    logger.LogInformation("Running Test-013 - get Device states list");

                    string deviceName = "projects/ingressdevelopmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Test-013-Device";

                    // First create a device to add config versions
                    var resultPre = await mClient.CreateDevice(4, "Test-013-Device", deviceName);
                    if (!resultPre.Item1 || (resultPre.Item2 == null))
                        logger.LogError("Test-013 - Failed");

                    // Next set some state information
                    var data = new DeviceSetStateRequestModel
                    {
                        State = new DeviceStateModel { BinaryData = "QUJD" }
                    };
                    var resultPre1 = await mClient.DeviceSetState(4, deviceName, data);
                    if (!resultPre1)
                        logger.LogError("Test-013 - Failed - Failed while setting state");

                    // further set one more state
                    data = new DeviceSetStateRequestModel
                    {
                        State = new DeviceStateModel { BinaryData = "QUMP" }
                    };
                    var resultPre2 = await mClient.DeviceSetState(4, deviceName, data);
                    if (!resultPre2)
                        logger.LogError("Test-013 - Failed - Failed while setting state");

                    // Actual test
                    var result013 = await mClient.GetDeviceStateList(4, deviceName, 5);
                    if (!result013.Item1 || (result013.Item2 == null))
                        logger.LogError("Test-013 - Failed to get Device states list");
                    else
                    {
                        // Verify obtained data
                        if (result013.Item2.DeviceStates.Count <= 0)
                            logger.LogError("Test-013 - Failed to get Device states list");
                        else
                        {

                            logger.LogInformation("Test-013 - Succeeded");
                        }
                    }
                    // Delete the newly created device - cleanup
                    await mClient.DeleteDevice(4, "Test-013-Device", deviceName);
                }

                //Console.ReadLine();
            }
        }

    }
}
