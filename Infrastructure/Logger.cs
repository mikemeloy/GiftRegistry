using System;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using Nop.Services.Logging;
using Nop.Core.Domain.Logging;

namespace i7MEDIA.Plugin.Widgets.Registry.Infrastructure;

public class Logger : ILogger_R
{
    private readonly ILogger _logger;

    public Logger(ILogger logger)
    {
        _logger = logger;
    }

    public async Task LogDebugAsync(string debugInfo)
    {
        await _logger.InsertLogAsync(logLevel: LogLevel.Debug, debugInfo);
    }

    public async Task LogErrorAsync(string error, Exception e)
    {
        await _logger.ErrorAsync($"Registry Plugin: {error}", e);
    }

    public async Task LogWarningAsync(string warning)
    {
        await _logger.WarningAsync($"Registry Plugin: {warning}");
    }
}
