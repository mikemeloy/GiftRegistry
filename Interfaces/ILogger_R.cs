using System;
using System.Threading.Tasks;

namespace i7MEDIA.Plugin.Widgets.Registry.Interfaces;

public interface ILogger_R
{
    Task LogErrorAsync(string error, Exception e);
    Task LogWarningAsync(string warning);
    Task LogDebugAsync(string debugInfo);
}
