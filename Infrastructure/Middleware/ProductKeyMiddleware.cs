using System.Linq;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using i7MEDIA.Plugin.Widgets.Registry.Services;
using i7MEDIA.Plugin.Widgets.Registry.Settings;
using Microsoft.AspNetCore.Http;

namespace i7MEDIA.Plugin.Widgets.Registry.Infrastructure.Middleware;

internal class ProductKeyMiddleware : IMiddleware
{
    private readonly ISettingsService_R _settingsService_R;
    private readonly IAdminService _adminService;
    public ProductKeyMiddleware(ISettingsService_R settingsService_R, IAdminService adminService)
    {
        _adminService = adminService;
        _settingsService_R = settingsService_R;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var settings = await _settingsService_R.GetSettingsAsync<RegistrySettings>();
        var validation = await _adminService.ValidateProductKeyAsync();

        if (!validation.IsValid)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync(validation.Errors?.FirstOrDefault()?.Message ?? "Invalid Product Key");
            return;
        }

        await next(context);
    }
}
