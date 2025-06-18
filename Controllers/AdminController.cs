using System.Collections.Generic;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace i7MEDIA.Plugin.Widgets.Registry.Controllers;

public class AdminController : BasePluginController
{
    private readonly IViewModelFactory _viewModelFactory;
    private readonly IAdminService _adminService;

    public AdminController(IViewModelFactory viewModelFactory, IAdminService adminService)
    {
        _viewModelFactory = viewModelFactory;
        _adminService = adminService;
    }

    [HttpGet]
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public IActionResult Index()
    {
        var model = _viewModelFactory.GetAdminViewModelAsync();

        return View("~/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Admin/Views/Report/Index.cshtml", model);
    }

    [HttpGet]
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public async Task<IEnumerable<RegistryConsultantDTO>> ConsultantAsync()
    {
        return await _adminService.GetConsultantsAsync();
    }

    [HttpPost]
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public async Task ConsultantAsync(RegistryConsultantDTO consultant)
    {
        await _adminService.UpsertConsultantAsync(consultant);
    }
}