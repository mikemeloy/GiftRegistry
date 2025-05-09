using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Components;
using Nop.Services.Cms;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace i7MEDIA.Plugin.Widgets.Registry;

public class RegistryPlugin : BasePlugin, IWidgetPlugin
{
    public bool HideInWidgetList => false;
    private readonly ILocalizationService _localizationService;
    public RegistryPlugin(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
    }

    public Type GetWidgetViewComponent(string widgetZone)
    {
        if (widgetZone == PublicWidgetZones.ProductPriceTop)
        {
            return typeof(ProductAddComponent);
        }

        if (widgetZone == PublicWidgetZones.HeaderMenuBefore)
        {
            return typeof(RegistryLinkComponent);
        }

        return typeof(RegistryLinkComponent);
    }

    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string> {
            PublicWidgetZones.ProductPriceTop,
            PublicWidgetZones.HeaderAfter
        });
    }

    public override async Task InstallAsync()
    {
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            ["Registry.Link"] = "Registry",
            ["Registry.Product.Link"] = "Add to Registry"
        });

        await base.InstallAsync();
    }
}
