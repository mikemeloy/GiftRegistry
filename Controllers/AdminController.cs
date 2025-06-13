using i7MEDIA.Plugin.Widgets.Registry.Models;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace i7MEDIA.Plugin.Widgets.Registry.Controllers;

public class AdminController : BasePluginController
{
    [HttpGet]
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public IActionResult Index()
    {
        var model = new ListViewModel("1.0.0");

        return View("~/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Admin/Views/Report/Index.cshtml", model);
    }
}