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

        var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();

        logger.LogInformation("Starting Tests");

        //do the actual work here
        RunTestsAsync(serviceProvider, logger).GetAwaiter().GetResult();

        logger.LogInformation("All done!");
    }

    /// <summary>
    /// Method to run the tests asynchronously
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    static async Task RunTestsAsync(ServiceProvider serviceProvider, ILogger logger)
    {
        MainClient mClient = new MainClient(serviceProvider.GetServices<IDeviceService>().FirstOrDefault());

        // Test-001 - Obtain list of devices for a particular registry
        // TBD - Need to obtain the token from authorization service
        logger.LogInformation("Running Test-101 - Obtain list of devices for a particular registry");
        var result = await mClient.GetDevicesList(  "https://iot-sandbox.clearblade.com",
                                                    "f6e1d8b30cb0cd8fe8cf95d0dfd001",
                                                    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiJmZWUxZDhiMzBjODY5MmRiOTZjMGNiODllYzNmIiwic2lkIjoiNjg2MDExZGYtM2VhZS00NjYxLWFlNDYtMGUzNDk4NTBjYzdiIiwidXQiOjIsInR0IjoxLCJleHAiOi0xLCJpYXQiOjE2NjQ4MTcyNzl9.PuzZnogOYym0U7k130oTVqnNwt7RvVGq6G8JZ0SRrss",
                                                    "projects/ingressdevelopmentenv/locations/us-central1/registries/PD-103-Registry");
        if(result.Item1 == false)
            logger.LogInformation("Test-101 - Failed");
        else
            logger.LogInformation("Test-101 - Succeeded");

        Console.ReadLine();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        //we will configure logging here
        services.AddLogging(configure => configure.AddConsole())
                .AddTransient<IDeviceService, DeviceService>();
    }
}
