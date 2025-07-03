using i7MEDIA.Plugin.Widgets.Registry.Extensions;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;


namespace i7MEDIA.Plugin.Widgets.Registry.Components;

public interface IOrderDetails
{
    public string CustomOrderNumber { get; set; }
}

public class RegistryGiftReceiptComponent : NopViewComponent
{
    public IViewComponentResult Invoke(string widgetZone, object additionalData)
    {
        var orderDetails = additionalData.Cast<object, IOrderDetails>();

        return View("~/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Public/Views/GiftReceiptLink.cshtml");
    }
}