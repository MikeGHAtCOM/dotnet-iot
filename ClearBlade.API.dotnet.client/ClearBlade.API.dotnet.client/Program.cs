/*
 * Copyright (c) 2023 ClearBlade Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 *
 * Copyright (c) 2018 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 */
 
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
          //  RunTests.Execute(serviceProvider, logger);
            RunSamples.Execute(serviceProvider, logger);
        }
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        //we will configure logging here
        services.AddLogging(configure => configure.AddConsole())
                .AddSingleton<IDeviceService, DeviceService>()
                .AddSingleton<IRegistryService, RegistryService>()
                .AddSingleton<IAdminService, AdminService>();
    }
}
