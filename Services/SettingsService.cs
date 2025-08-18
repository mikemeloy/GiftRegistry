using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Configuration;
using Nop.Services.Configuration;

namespace i7MEDIA.Plugin.Widgets.Registry.Services;

public interface ISettingsService_R
{
    public Task SaveSettingsAsync<T>(T settings) where T : ISettings, new();
    public Task<T> GetSettingsAsync<T>() where T : ISettings, new();
}

public class SettingsService : ISettingsService_R
{
    protected readonly ISettingService _settingService;
    protected readonly IStoreContext _storeContext;

    public SettingsService(ISettingService settingService, IStoreContext storeContext)
    {
        _settingService = settingService;
        _storeContext = storeContext;
    }

    public async Task SaveSettingsAsync<T>(T settings) where T : ISettings, new()
    {
        await _settingService.SaveSettingAsync(settings);
    }

    public async Task<T> GetSettingsAsync<T>() where T : ISettings, new()
    {
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var setting = await _settingService.LoadSettingAsync<T>(storeScope);

        return setting;
    }
}
