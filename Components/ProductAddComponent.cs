using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using i7MEDIA.Plugin.Widgets.Registry.Models;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;
using Nop.Web.Framework.Models;

namespace i7MEDIA.Plugin.Widgets.Registry.Components;

public class ProductAddComponent : NopViewComponent
{
    private readonly IRegistryService _registryService;
    public ProductAddComponent(IRegistryService registryService)
    {
        _registryService = registryService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, BaseNopEntityModel additionalData)
    {
        var model = new ProductLinkViewModel("1.0.0", additionalData.Id)
        {
            Registries = await _registryService.GetCurrentCustomerRegistriesAsync(),
            RequiredAttributes = await _registryService.GetProductAttributesByProductIdAsync(additionalData.Id)
        };

        return View("~/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Public/Views/ProductLink.cshtml", model);
    }
}