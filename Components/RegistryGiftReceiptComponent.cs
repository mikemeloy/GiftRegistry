using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;


namespace i7MEDIA.Plugin.Widgets.Registry.Components;

public interface IOrderDetails
{
    public string CustomOrderNumber { get; set; }
}

public class RegistryGiftReceiptComponent : NopViewComponent
{
    private readonly IViewModelFactory _viewModelFactory;
    public RegistryGiftReceiptComponent(IViewModelFactory viewModelFactory)
    {
        _viewModelFactory = viewModelFactory;
    }

    public IViewComponentResult Invoke(string widgetZone, dynamic additionalData)
    {
        var model = _viewModelFactory.GetRegistryGiftReceiptViewModel(additionalData.CustomOrderNumber);

        return View("~/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Public/Views/GiftReceiptLink.cshtml", model);
    }
}