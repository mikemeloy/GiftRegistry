using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Components;
using Microsoft.AspNetCore.Routing;
using Nop.Services.Cms;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;

namespace i7MEDIA.Plugin.Widgets.Registry;

public class RegistryPlugin : BasePlugin, IWidgetPlugin, IAdminMenuPlugin
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

    public async Task ManageSiteMapAsync(SiteMapNode rootNode)
    {
        var config = rootNode.ChildNodes.FirstOrDefault(node => node.SystemName.Equals("Customers"));

        if (config == null)
        {
            return;
        }

        config.ChildNodes.Insert(config.ChildNodes.Count, new SiteMapNode()
        {
            Visible = true,
            ControllerName = "Registry",
            ActionName = "Report",
            SystemName = "Registry plugin",
            Title = await _localizationService.GetResourceAsync("Registry.Link"),
            IconClass = "far fa-dot-circle",
            //RouteValues = new RouteValueDictionary { { "area", AreaNames.Admin } }
        });
    }
}