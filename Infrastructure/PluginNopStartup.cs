using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using i7MEDIA.Plugin.Widgets.Registry.Repositories;
using i7MEDIA.Plugin.Widgets.Registry.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nop.Core.Infrastructure;

namespace i7MEDIA.Plugin.Widgets.Registry.Infrastructure;

public class PluginNopStartup : INopStartup
{
    public int Order => 99;

    public void Configure(IApplicationBuilder application) { }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        AddCustomServices(services);
        AddNopServices(services);
    }

    private static void AddCustomServices(IServiceCollection services)
    {
        services.AddScoped<IRegistryRepository, RegistryRepository>();
        services.AddScoped<IRegistryService, RegistryService>();
        services.AddScoped<ILogger_R, Logger>();

    }

    private static void AddNopServices(IServiceCollection services)
    {
        services.TryAddScoped<INopServices, NopServices>();
    }
}