using System.Collections.Generic;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using i7MEDIA.Plugin.Widgets.Registry.Models;
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

        return View("~/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Admin/Views/Index.cshtml", model);
    }

    [HttpGet(template: "Admin/Registry/List")]
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public async Task<IActionResult> RegistryListAsync()
    {
        var model = await _viewModelFactory.GetRegistryPartialViewModelAsync();

        return View("~/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Admin/Views/_RegistryList.cshtml", model);
    }

    [HttpGet(template: "Admin/Registry/Query")]
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public async Task<IActionResult> RegistryQueryAsync(string query)
    {
        var model = await _viewModelFactory.GetRegistryRowPartialViewModelAsync(query);

        return View("~/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Admin/Views/_RegistryRow.cshtml", model);
    }

    [HttpGet(template: "Admin/Consultant/List")]
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public async Task<IActionResult> ConsultantListAsync()
    {
        var model = await _viewModelFactory.GetConsultantPartialViewModelAsync();

        return View("~/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Admin/Views/_RegistryConsultant.cshtml", model);
    }

    [HttpGet(template: "Admin/RegistryType/List")]
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public async Task<IActionResult> RegistryTypeListAsync()
    {
        var model = await _viewModelFactory.GetRegistryTypePartialViewModelAsync();

        return View("~/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Admin/Views/_RegistryType.cshtml", model);
    }

    [HttpGet(template: "Admin/ShippingOption/List")]
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public async Task<IActionResult> RegistryShippingListAsync()
    {
        var model = await _viewModelFactory.GetRegistryShippingOptionViewModelAsync();

        return View("~/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Admin/Views/_RegistryShippingOption.cshtml", model);
    }

    [HttpGet]
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public async Task<RegistryList> RegistryAsync(string query)
    {
        return await _adminService.QueryAsync(query);
    }

    [HttpGet]
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public async Task<IEnumerable<RegistryConsultantDTO>> ConsultantAsync()
    {
        return await _adminService.GetConsultantsAsync();
    }

    [HttpGet]
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public async Task<IEnumerable<RegistryTypeDTO>> RegistryTypeAsync()
    {
        return await _adminService.GetRegistryTypesAsync();
    }

    [HttpGet]
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public async Task<IEnumerable<RegistryShippingOptionDTO>> ShippingOptionAsync()
    {
        return await _adminService.GetShippingOptionsAsync();
    }

    [HttpPost]
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public async Task ConsultantAsync([FromBody] RegistryConsultantDTO consultant)
    {
        await _adminService.UpsertConsultantAsync(consultant);
    }

    [HttpPost]
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public async Task RegistryTypeAsync([FromBody] RegistryTypeDTO registryType)
    {
        await _adminService.UpsertRegistryTypeAsync(registryType);
    }

    [HttpPost]
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public async Task ShippingOptionAsync([FromBody] RegistryShippingOptionDTO registryType)
    {
        await _adminService.UpsertRegistryShippingOptionAsync(registryType);
    }
}