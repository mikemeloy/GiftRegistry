using i7MEDIA.Plugin.Widgets.Registry.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Nop.Web.Framework.Components;
using Nop.Web.Framework.Models;

namespace i7MEDIA.Plugin.Widgets.Registry.Components;

public class RegistryAdminComponent : NopViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, BaseNopEntityModel additionalData)
    {
        var model = new RegistryAdminViewModel();

        return View("~/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Public/Views/ProductLink.cshtml", model);
    }
}
