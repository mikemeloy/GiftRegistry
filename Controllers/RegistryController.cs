using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Extensions;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using i7MEDIA.Plugin.Widgets.Registry.Models;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Controllers;

namespace i7MEDIA.Plugin.Widgets.Registry.Controllers;

public class RegistryController : BasePluginController
{
    private readonly IRegistryService _registryService;
    public RegistryController(IRegistryService registryService)
    {
        _registryService = registryService;
    }

    [HttpGet]
    [IgnoreAntiforgeryToken]
    public IActionResult Index()
    {
        var model = new RegistryModel("1.0.0");

        return View("~/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Public/Views/Index.cshtml", model);
    }

    [HttpGet]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> ListAsync(string query)
    {
        var model = await _registryService.Query(query);

        return View("~/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Public/Views/List.cshtml", model);
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<bool> Save([FromBody] RegistryAddProductRequest request)
    {
        if (!request.IsValid())
        {
            return false;
        }

        var success = await _registryService.InsertRegistryItemAsync(request.GiftRegistryId, request.ProductId);

        return success;
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<bool> InsertAsync([FromBody] RegistryInsertRequest request)
    {
        if (request.IsNull())
        {
            return false;
        }

        var success = await _registryService.InsertCustomerRegistryAsync(request.Name, request.Description, request.EventDate);

        return success;
    }
}
