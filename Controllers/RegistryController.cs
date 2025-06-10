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
        var model = new PluginModel("1.0.0");

        return View("~/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Public/Views/Index.cshtml", model);
    }

    [HttpGet]
    [IgnoreAntiforgeryToken]
    public IActionResult Report()
    {
        var model = new PluginModel("1.0.0");

        return View("~/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Admin/Views/Report/Index.cshtml", model);
    }

    [HttpGet]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> List(string query)
    {
        var model = await _registryService.Query(query);

        return View("~/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Public/Views/List.cshtml", model);
    }

    [HttpGet]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> RegistryAsync(int? id)
    {
        if (id.IsNull())
        {
            return View();
        }

        var model = await _registryService.GetCustomerRegistryByIdAsync(id.Value);

        return View("~/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Public/Views/Display.cshtml", model);
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<bool> SaveAsync([FromBody] RegistryAddProductRequest request)
    {
        if (!request.IsValid())
        {
            return false;
        }

        var success = await _registryService.InsertRegistryItemAsync(request.GiftRegistryId, request.ProductId, request.Quantity);

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

        var success = await _registryService.InsertCustomerRegistryAsync(request.Name, request.Description, request.EventDate, request.Notes);

        return success;
    }

    [HttpDelete]
    [IgnoreAntiforgeryToken]
    public async Task<bool> DeleteAsync(int? id)
    {
        if (id.IsNull())
        {
            return false;
        }

        var success = await _registryService.DeleteRegistryAsync(id.Value);

        return success;
    }

    [HttpDelete]
    [IgnoreAntiforgeryToken]
    public async Task<bool> DeleteItemAsync(int? id)
    {
        if (id.IsNull())
        {
            return false;
        }

        var success = await _registryService.DeleteRegistryItemAsync(id.Value);

        return success;
    }

    [HttpPost]
    public async Task<bool> AddToCartAsync([FromBody] int? registryItemId)
    {
        if (registryItemId.IsNull())
        {
            return false;
        }

        await _registryService.AddRegistryItemToCart(registryItemId.Value);

        return true;
    }
}

public record AddToCartModel(int RegistryItemId);