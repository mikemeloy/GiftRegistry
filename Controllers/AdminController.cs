using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace i7MEDIA.Plugin.Widgets.Registry.Controllers;

public class AdminController : BasePluginController
{
    private readonly IViewModelFactory _viewModelFactory;

    public AdminController(IViewModelFactory viewModelFactory)
    {
        _viewModelFactory = viewModelFactory;
    }

    [HttpGet]
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public IActionResult Index()
    {
        var model = _viewModelFactory.GetAdminViewModelAsync();

        return View("~/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Admin/Views/Report/Index.cshtml", model);
    }
}