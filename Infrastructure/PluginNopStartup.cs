using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using i7MEDIA.Plugin.Widgets.Registry.Repositories;
using i7MEDIA.Plugin.Widgets.Registry.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;

namespace i7MEDIA.Plugin.Widgets.Registry.Infrastructure;

public class PluginNopStartup : INopStartup
{
    public int Order => 99;

    public void Configure(IApplicationBuilder application) { }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        AddCustomServices(services);
    }

    private static void AddCustomServices(IServiceCollection services)
    {
        services.AddScoped<IRegistryRepository, RegistryRepository>();
        services.AddScoped<IRegistryService, RegistryService>();
        services.AddScoped<ILogger_R, Logger>();
    }
}
