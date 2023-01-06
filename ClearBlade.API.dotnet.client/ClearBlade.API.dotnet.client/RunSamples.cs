using ClearBlade.API.dotnet.client.core;
using ClearBlade.API.dotnet.client.core.Models;
using ClearBlade.API.dotnet.client.core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ClearBlade.API.dotnet.client
{
    /// <summary>
    /// Helper class to run and debug samples
    /// </summary>
    public static class RunSamples
    {
        #region Test Selection
        static readonly bool bGetDevicesList = false;
        static readonly bool bSendCommandToDevice = false;
        static readonly bool bModifyCloudToDeviceConfig = false;
        static readonly bool bCreateDevice = false;
        static readonly bool bDeleteDevice = false;
        static readonly bool bGetDevice = false;
        static readonly bool bGetDeviceConfig = false;
        static readonly bool bBindUnBindDevice = false;
        static readonly bool bGetRegistryConfig = false;
        static readonly bool bPatchRegistryConfig = false;
        static readonly bool bPatchDeviceConfig = false;
        static readonly bool bGetDeviceConfigVersions = false;
        static readonly bool bGetDeviceStates = false;
        static readonly bool bCreateDeviceRSA = false;
        static readonly bool bCreateDeviceEC = false;
        static readonly bool bCreateRegistry = false;
        static readonly bool bDeleteRegistry = false;
        static readonly bool bGetDevicesListGateway = false;
        static readonly bool bGetGateways = false;
        static readonly bool bUnbindAndDelete = false;
        #endregion

        public static bool Execute(ServiceProvider serviceProvider, ILogger logger)
        {
            // Set which sample to run
            // bGetDeviceStates = true;

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
            IRegistryService? registryService = serviceProvider.GetService<IRegistryService>();
            if (deviceService != null && registryService != null)
            {
                MainClient mClient = new MainClient(deviceService, registryService);

                // Sample - Obtain list of devices for a particular registry
                if (bGetDevicesList)
                {
                    logger.LogInformation("Obtain list of devices for a particular registry");
                    var result = await mClient.GetDevicesList(4, "projects/developmentenv/locations/us-central1/registries/Sample-New-Registry", null);
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

                    var result = await mClient.SendCommandToDevice(4, "projects/developmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Sample-New-Device", data);
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
                    var result = await mClient.ModifyCloudToDeviceConfig(4, "projects/developmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Sample-New-Device", data);
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
                    string name = "projects/developmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Sample-New-Device";

                    var result = await mClient.CreateDevice(4, id, name, null, null);
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
                    string name = "projects/developmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Sample-New-Device";

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

                    string name = "projects/developmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Sample-New-Device";

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

                    string name = "projects/developmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Sample-New-Device";
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
                    var result = await mClient.BindDeviceToGateway(4, "projects/developmentenv/locations/us-central1/registries/Sample-New-Registry", "TestGateway", "Sample-New-Device");
                    if (!result)
                    {
                        logger.LogError("Failed To Bind Device");
                    }
                    else
                    {
                        // Actual test - UnBind Device
                        result = await mClient.UnBindDeviceFromGateway(4, "projects/developmentenv/locations/us-central1/registries/Sample-New-Registry", "TestGateway", "Sample-New-Device");
                        if (!result)
                            logger.LogError("Failed to unbind a device");
                        else
                            logger.LogInformation("Successfully unbind device");

                    }
                }

                // Sample - Get configuration of a registry
                if (bGetRegistryConfig)
                {
                    logger.LogInformation("Get configuration of a registry");

                    // While running this sample, it is assumed that, registry with name
                    // "Sample-New-Registry" exists and version is updated to "2"

                    string name = "projects/developmentenv/locations/us-central1/registries/Sample-New-Registry";

                    var result = await mClient.GetRegistryConfig(4, name);
                    if (!result.Item1 || (result.Item2 == null))
                        logger.LogError("Failed to get a registry configuration");
                    else
                    {
                        logger.LogInformation("Successfully obtained the registry configuration");

                        // Use the obtained information
                    }
                }

                // Sample - Update configuration of a registry
                if (bPatchRegistryConfig)
                {
                    logger.LogInformation("Update configuration of a registry");

                    // While running this sample, it is assumed that, registry with name
                    // "Sample-New-Registry" exists

                    string name = "projects/developmentenv/locations/us-central1/registries/Sample-New-Registry";

                    var result = await mClient.GetRegistryConfig(4, name);
                    if (!result.Item1 || (result.Item2 == null))
                        logger.LogError("Failed to get a registry configuration");
                    else
                    {
                        logger.LogInformation("Successfully obtained the registry configuration");

                        // Use the obtained information
                        // Following are the valid mask entries -
                        // eventNotificationConfigs
                        // stateNotificationConfig.pubsub_topic_name
                        // mqttConfig.mqtt_enabled_state
                        // httpConfig.http_enabled_state
                        // logLevel
                        // credentials
                        string updateMask = "httpConfig.http_enabled_state,mqttConfig.mqtt_enabled_state";
                        result.Item2.MqttConfig.MqttEnabledState = "MQTT_ENABLED";
                        result.Item2.HttpConfig.HttpEnabledState = "HTTP_ENABLED";

                        result = await mClient.PatchRegistry(4, name, updateMask, result.Item2);

                        if (!result.Item1 || (result.Item2 == null))
                            logger.LogError("Failed to update a registry configuration");
                    }
                }

                // Sample - Update configuration of a device
                if (bPatchDeviceConfig)
                {
                    logger.LogInformation("Update configuration of a device");

                    // While running this sample, it is assumed that, device with name
                    // "Sample-New-Device" exists

                    string name = "projects/developmentenv/locations/us-central1/registries/Sample-New-Registry/devices/Sample-New-Device";

                    var result = await mClient.GetDevice(4, name);
                    if (!result.Item1 || (result.Item2 == null))
                        logger.LogError("Failed to get a device configuration");
                    else
                    {
                        logger.LogInformation("Successfully obtained the device configuration");

                        // Use the obtained information
                        string updateMask = "metadata";
                        string pubKey = "-----BEGIN PUBLIC KEY-----\nMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA5P0Z4OUD5PSjri8xexGo\n6eQ39NGyQbXamIgWAwvnAs/oDRVqEejE2nwDhnpykaCGLkuDEN0LPd2wF+vC2Cq3\nY3YvkJh71IkjuAjMZQ+00CXdezfCjmTtEpMCNA3cV+G1g6uIcdEpHKs0YHfC9CFQ\nrjkc7tl3idmcQLngIov/gsFY7D1pbOgkCVVcZCRLgsdFfhCUYwYCvdEVJP3w+5mG\nybvmhNRbbFG7eG3+hmZoOg0h3f6r2fqgSx6l0+Z3D77SRT6lBEHvGDlxb08ASeuE\n0SJAc6PdAKd3FDqdZok4z1qJsgMqtU/ZGJJG54pNECWmhoOar+aQmmqnZ6kGQ5cn\nEwIDAQAB\n-----END PUBLIC KEY-----\n";
                        result.Item2.Credentials.Add(new DeviceCredential
                        {
                            ExpirationTime = "",
                            PublicKey = new PublicKeyCredential
                            {
                                Format = "RSA_PEM",
                                Key = pubKey
                            }
                        });

                        result = await mClient.PatchDevice(4, name, updateMask, result.Item2);

                        if (!result.Item1 || (result.Item2 == null))
                            logger.LogError("Failed to update a device configuration");
                    }
                }

                // Sample - Get list of configuration versions of a device
                if (bGetDeviceConfigVersions)
                {
                    logger.LogInformation("Get configuration versions list for a device");

                    // While running this sample, it is assumed that, device with name
                    // "Sample-New-Device" exists

                    string name = "projects/developmentenv/locations/us-central1/registries/Sample-New-Registry/devices/Sample-New-Device";

                    var result = await mClient.GetDeviceConfigVersionList(4, name, 5);
                    if (!result.Item1 || (result.Item2 == null))
                        logger.LogError("Failed to get a device configuration");
                    else
                    {
                        logger.LogInformation("Successfully obtained the list of device configuration versions");

                        // Use the obtained information
                    }
                }

                // Sample - Get list of states of a device
                if (bGetDeviceStates)
                {
                    logger.LogInformation("Get states list for a device");

                    // While running this sample, it is assumed that, device with name
                    // "Sample-New-Device" exists

                    string name = "projects/developmentenv/locations/us-central1/registries/Sample-New-Registry/devices/Sample-New-Device";

                    var result = await mClient.GetDeviceStateList(4, name, 5);
                    if (!result.Item1 || (result.Item2 == null))
                        logger.LogError("Failed to get a device states");
                    else
                    {
                        logger.LogInformation("Successfully obtained the list of device configuration versions");

                        // Use the obtained information
                    }
                }

                //NEW NEW NEW

                // Sample - Create a device with RSA credentials
                if (bCreateDeviceRSA)
                {
                    logger.LogInformation("Create a new device with RSA credentials");

                    string id = "Sample-New-Device";
                    string name = "projects/developmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Sample-New-Device";

                    String keyText = File.ReadAllText("path/to/key");

                    var credentials = new List<DeviceCredential>
                    {
                        new DeviceCredential()
                        {
                            PublicKey = new PublicKeyCredential()
                            {
                                Key = keyText,
                                Format = "RSA_X509_PEM"
                            },
                        }
                    };

                    var result = await mClient.CreateDevice(4, id, name, credentials, null);
                    if (!result.Item1 || (result.Item2 == null))
                        logger.LogError("Failed to create new device");
                    else
                    {
                        logger.LogInformation("Successfully created new device");

                        // The result.Item2 object can be used to refer to newly created device
                    }
                }

                // Sample - Create a device with Elliptic Curve(EC) credentials
                if (bCreateDeviceEC)
                {
                    logger.LogInformation("Create a new device with EC credentials");

                    string id = "Sample-New-Device";
                    string name = "projects/developmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Sample-New-Device";

                    String keyText = File.ReadAllText("path/to/key");

                    var credentials = new List<DeviceCredential>
                    {
                        new DeviceCredential()
                        {
                            PublicKey = new PublicKeyCredential()
                            {
                                Key = keyText,
                                Format = "ES256_PEM"
                            },
                        }
                    };

                    var result = await mClient.CreateDevice(4, id, name, credentials, null);
                    if (!result.Item1 || (result.Item2 == null))
                        logger.LogError("Failed to create new device");
                    else
                    {
                        logger.LogInformation("Successfully created new device");

                        // The result.Item2 object can be used to refer to newly created device
                    }
                }

                // Sample - Create a registry
                if (bCreateRegistry)
                {
                    logger.LogInformation("Create a registry");

                    string parent = "projects/developmentenv/locations/us-central1";
                    string name = "Sample-New-Registry-2";
                    string pubsubTopic = "projects/developmentenv/topics/pubsubTopic";

                    RegistryConfigModel registryConfigModel = new RegistryConfigModel()
                    {
                        Id = name,
                    };

                    registryConfigModel.EventNotificationConfigs = new List<EventNotificationConfig>();
                    var toAdd = new EventNotificationConfig()
                    {
                        PubsubTopicName = pubsubTopic
                    };
                    registryConfigModel.EventNotificationConfigs.Add(toAdd);

                    var result = await mClient.CreateRegistry(4, parent, registryConfigModel);
                    if (!result.Item1 || (result.Item2 == null))
                        logger.LogError("Failed to get a device states");
                    else
                    {
                        logger.LogInformation("Successfully obtained the list of device configuration versions");

                        // Use the obtained information
                    }
                }

                // Sample - Delete a registry
                if (bDeleteRegistry)
                {
                    logger.LogInformation("Delete a registry");

                    // While running this sample, it is assumed that, registry with name
                    // "Sample-New-Registry-2" exists

                    string name = "Sample-New-Registry-2";

                    var result = await mClient.DeleteRegistry(4, name);
                    if (!result)
                        logger.LogError("Failed to delete a registry");
                    else
                    {
                        logger.LogInformation("Successfully deleted a registry");
                    }
                }

                // Sample - List all devices bound to the gateway
                if (bGetDevicesListGateway)
                {
                    logger.LogInformation("Obtain list of devices for a particular registry and gateway");

                    // While running this sample, it is assumed that, gateway with name
                    // "associated-gateway" exists

                    GatewayListOptionsModel gatewayListOptions = new GatewayListOptionsModel()
                    {
                        AssociationsGatewayId = "associated-gateway",
                    };
                    var result = await mClient.GetDevicesList(4, "projects/developmentenv/locations/us-central1/registries/Sample-New-Registry", gatewayListOptions);
                    if (!result.Item1)
                        logger.LogError("Failed to get list of devices");
                    else
                    {
                        logger.LogInformation("Succeeded in getting the list of Devices");

                        // Use the list
                    }
                }

                // Sample - List all gateways in a registry
                if (bGetGateways)
                {
                    logger.LogInformation("Obtain list of gateways in a registry");

                    GatewayListOptionsModel gatewayListOptions = new GatewayListOptionsModel()
                    {
                        GatewayType = core.Enums.GatewayTypeEnum.GATEWAY,
                    };

                    var result = await mClient.GetDevicesList(4, "projects/developmentenv/locations/us-central1/registries/Sample-New-Registry", gatewayListOptions);
                    if (!result.Item1)
                        logger.LogError("Failed to get list of devices");
                    else
                    {
                        logger.LogInformation("Succeeded in getting the list of gateways");

                        // Use the list
                    }
                }

                // Sample - Delete a device bound to a gateway
                if (bUnbindAndDelete)
                {
                    logger.LogInformation("Unbind the device");

                    var unbindResult = await mClient.UnBindDeviceFromGateway(4, "projects/developmentenv/locations/us-central1/registries/Sample-New-Registry", "TestGateway", "Sample-New-Device");
                    if (!unbindResult)
                        logger.LogError("Failed to unbind a device");
                    else
                        logger.LogInformation("Successfully unbind device");

                    logger.LogInformation("Delete the device");

                    string id = "Sample-New-Device";
                    string name = "projects/developmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Sample-New-Device";

                    var deleteResult = await mClient.DeleteDevice(4, id, name);
                    if (!deleteResult.Item1 || (deleteResult.Item2 == null))
                        logger.LogError("Failed to delete device");
                    else
                    {
                        logger.LogInformation("Successfully deleted the device");
                    }
                }

                // Sample - Create a gateway
                if (bCreateDeviceEC)
                {
                    logger.LogInformation("Create a new gateway");

                    string id = "Sample-New-Gateway";
                    string name = "projects/developmentenv/locations/us-central1/registries/Sample-New-Registry/Devices/Sample-New-Gateway";

                    String keyText = File.ReadAllText("path/to/key");

                    var credentials = new List<DeviceCredential>
                    {
                        new DeviceCredential()
                        {
                            PublicKey = new PublicKeyCredential()
                            {
                                Key = keyText,
                                Format = "ES256_PEM"
                            },
                        }
                    };

                    var gatewayConfig = new GatewayConfig()
                    {
                        GatewayType = "GATEWAY",
                        GatewayAuthMethod = "ASSOCIATION_ONLY"
                    };

                    var result = await mClient.CreateDevice(4, id, name, credentials, gatewayConfig);
                    if (!result.Item1 || (result.Item2 == null))
                        logger.LogError("Failed to create new device");
                    else
                    {
                        logger.LogInformation("Successfully created new device");

                        // The result.Item2 object can be used to refer to newly created device
                    }
                }
                //Console.ReadLine();
            }
        }
    }
}
