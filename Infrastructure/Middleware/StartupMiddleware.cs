using System;
using i7MEDIA.Plugin.Widgets.Registry.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace i7MEDIA.Plugin.Widgets.Registry.Infrastructure.Middleware;

internal class StartupMiddleware : IStartupFilter
{
    private readonly IUtils _utils;
    public StartupMiddleware(IUtils utils)
    {
        _utils = utils;
    }

    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return builder =>
        {
            _utils.ResetProductKeyValidAsync();

            next(builder);
        };
    }
}