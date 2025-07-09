using i7MEDIA.Plugin.Widgets.Registry.Factories;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using i7MEDIA.Plugin.Widgets.Registry.Repositories;
using i7MEDIA.Plugin.Widgets.Registry.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nop.Core.Infrastructure;
using Nop.Services.Catalog;
using Nop.Services.Orders;

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
        services.AddScoped<ILogger_R, Logger>();
        services.TryAddScoped<INopServices, NopServices>();
        services.AddScoped<IRegistryService, RegistryService>();
        services.AddScoped<IRegistryRepository, RegistryRepository>();
        services.AddScoped<IViewModelFactory, ViewModelFactory>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IRegistryPdfService, PdfService>();
    }

    private static void AddNopServices(IServiceCollection services)
    {
        services.TryAddScoped<IProductService, ProductService>();
        services.TryAddScoped<IShoppingCartService, ShoppingCartService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }
}