using ClearBlade.API.dotnet.client.core;
using ClearBlade.API.dotnet.client.core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class Program
{
    public static void Main(string[] args)
    {
        //setup our DI
        var serviceCollection = new ServiceCollection();

        ConfigureServices(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider(); 

        var logger = serviceProvider?.GetService<ILoggerFactory>()?.CreateLogger<Program>();
        if(logger != null)
            logger.LogInformation("Starting Tests");

        //do the actual work here
        if ((serviceProvider != null) && (logger != null))
        {
            RunTestsAsync(serviceProvider, logger).GetAwaiter().GetResult();

            logger.LogInformation("All done!");
        }
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
            // TBD - Need to obtain the token from authorization service
            logger.LogInformation("Running Test-001 - Obtain list of devices for a particular registry");
            var result = await mClient.GetDevicesList(4, "https://iot-sandbox.clearblade.com",
                                                        "f6e1d8b30cb0cd8fe8cf95d0dfd001",
                                                        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiJmZWUxZDhiMzBjODY5MmRiOTZjMGNiODllYzNmIiwic2lkIjoiNjg2MDExZGYtM2VhZS00NjYxLWFlNDYtMGUzNDk4NTBjYzdiIiwidXQiOjIsInR0IjoxLCJleHAiOi0xLCJpYXQiOjE2NjQ4MTcyNzl9.PuzZnogOYym0U7k130oTVqnNwt7RvVGq6G8JZ0SRrss",
                                                        "projects/ingressdevelopmentenv/locations/us-central1/registries/PD-103-Registry");
            if (!result.Item1)
                logger.LogInformation("Test-001 - Failed");
            else
            {
                logger.LogInformation("Test-001 - Succeeded");

                // TBD - Verification of result. This can be done after the end point
                // for Create Device is done. So that, we can create a device, get list
                // and verify that it exists.
            }

            // Test-002 - Send Command to Device
            // TBD - Need to obtain the token from authorization service
            logger.LogInformation("Running Test-002 - Send Command to Device");
            var data = new
            {
                binaryData = "QUJD",
                versionToUpdate = "1"
            };
            var result002 = await mClient.SendCommandToDevice(4, "https://iot-sandbox.clearblade.com",
                                                        "f6e1d8b30cb0cd8fe8cf95d0dfd001",
                                                        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiJmZWUxZDhiMzBjODY5MmRiOTZjMGNiODllYzNmIiwic2lkIjoiNjg2MDExZGYtM2VhZS00NjYxLWFlNDYtMGUzNDk4NTBjYzdiIiwidXQiOjIsInR0IjoxLCJleHAiOi0xLCJpYXQiOjE2NjQ4MTcyNzl9.PuzZnogOYym0U7k130oTVqnNwt7RvVGq6G8JZ0SRrss",
                                                        "PD-103-Device", data);
            if (!result002)
                logger.LogInformation("Test-002 - Failed");
            else
            {
                logger.LogInformation("Test-002 - Succeeded");
            }

            // Test-003 - Send Command to Device
            // TBD - Need to obtain the token from authorization service
            logger.LogInformation("Running Test-003 - Modify device config");
            data = new
            {
                binaryData = "QUJD",
                versionToUpdate = "19"
            };
            var result003 = await mClient.ModifyCloudToDeviceConfig(4, "https://iot-sandbox.clearblade.com",
                                                        "f6e1d8b30cb0cd8fe8cf95d0dfd001",
                                                        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiJmZWUxZDhiMzBjODY5MmRiOTZjMGNiODllYzNmIiwic2lkIjoiNjg2MDExZGYtM2VhZS00NjYxLWFlNDYtMGUzNDk4NTBjYzdiIiwidXQiOjIsInR0IjoxLCJleHAiOi0xLCJpYXQiOjE2NjQ4MTcyNzl9.PuzZnogOYym0U7k130oTVqnNwt7RvVGq6G8JZ0SRrss",
                                                        "PD-103-Device", data);
            if (!result003)
                logger.LogInformation("Test-003 - Failed");
            else
            {
                logger.LogInformation("Test-003 - Succeeded");
            }


            //Console.ReadLine();
        }
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        //we will configure logging here
        services.AddLogging(configure => configure.AddConsole())
                .AddTransient<IDeviceService, DeviceService>();
    }
}
