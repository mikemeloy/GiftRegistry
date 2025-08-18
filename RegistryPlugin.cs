using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Components;
using Microsoft.AspNetCore.Routing;
using Nop.Core;
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
    protected readonly IWebHelper _webHelper;

    public RegistryPlugin(ILocalizationService localizationService, IWebHelper webHelper)
    {
        _localizationService = localizationService;
        _webHelper = webHelper;
    }

    public override string GetConfigurationPageUrl()
    {
        var path = $"{_webHelper.GetStoreLocation()}Admin/RegistryPlugin/Configure";
        return path;
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

        if (widgetZone == PublicWidgetZones.OrderDetailsPageTop)
        {
            return typeof(RegistryGiftReceiptComponent);
        }

        if (widgetZone == AdminWidgetZones.OrderDetailsButtons)
        {
            return typeof(RegistryGiftReceiptComponent);
        }

        return typeof(RegistryLinkComponent);
    }

    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string> {
            PublicWidgetZones.ProductPriceTop,
            PublicWidgetZones.HeaderAfter,
            PublicWidgetZones.OrderDetailsPageTop,
            AdminWidgetZones.OrderDetailsButtons
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
            ControllerName = "Admin",
            ActionName = "Index",
            SystemName = "Registry plugin",
            Title = await _localizationService.GetResourceAsync("Registry.Link"),
            IconClass = "far fa-dot-circle",
            RouteValues = new RouteValueDictionary { { "area", AreaNames.Admin } }
        });
    }
}