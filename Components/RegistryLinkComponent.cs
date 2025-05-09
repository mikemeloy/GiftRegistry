using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace i7MEDIA.Plugin.Widgets.Registry.Components;

public class RegistryLinkComponent : NopViewComponent
{
    public RegistryLinkComponent()
    {

    }

    public IViewComponentResult Invoke(string widgetZone, object additionalData)
    {
        return View("~/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Public/Views/RegistryLink.cshtml");
    }
}