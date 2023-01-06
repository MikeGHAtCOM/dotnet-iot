using ClearBlade.API.dotnet.client;
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

        //do the actual work here
        if ((serviceProvider != null) && (logger != null))
        {
            RunTests.Execute(serviceProvider, logger);
            RunSamples.Execute(serviceProvider, logger);
        }
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        //we will configure logging here
        services.AddLogging(configure => configure.AddConsole())
                .AddSingleton<IDeviceService, DeviceService>()
                .AddSingleton<IAdminService, AdminService>();
    }
}
