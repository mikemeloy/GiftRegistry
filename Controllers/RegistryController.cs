using System.Collections.Generic;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;
using i7MEDIA.Plugin.Widgets.Registry.Extensions;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using i7MEDIA.Plugin.Widgets.Registry.Models;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Controllers;

namespace i7MEDIA.Plugin.Widgets.Registry.Controllers;

public class RegistryController : BasePluginController
{
    private readonly IRegistryService _registryService;
    private readonly IViewModelFactory _viewModelFactory;
    public RegistryController(IRegistryService registryService, IViewModelFactory viewModelFactory)
    {
        _registryService = registryService;
        _viewModelFactory = viewModelFactory;
    }

    [HttpGet]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Index()
    {
        var model = await _viewModelFactory.GetListViewModelAsync();

        return View("~/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Public/Views/Index.cshtml", model);
    }

    [HttpGet]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> ListAsync(string query)
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

    [HttpGet]
    [IgnoreAntiforgeryToken]
    public async Task<RegistryViewModel> GetAsync(int? id)
    {
        if (id.IsNull())
        {
            return null;
        }

        return await _registryService.GetCustomerRegistryByIdAsync(id.Value);
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<bool> SaveAsync([FromBody] RegistryAddProductRequest request)
    {
        if (!request.IsValid())
        {
            return false;
        }

        return await _registryService.InsertRegistryItemAsync(request.GiftRegistryId, request.ProductId, request.Quantity);
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<bool> UpdateAsync([FromBody] RegistryDTO registryDTO)
    {
        if (registryDTO.IsNull())
        {
            return false;
        }

        var success = await _registryService.UpdateCustomerRegistryAsync(registryDTO);

        return success;
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<bool> InsertAsync([FromBody] RegistryDTO registryDTO)
    {
        if (registryDTO.IsNull())
        {
            return false;
        }

        var success = await _registryService.InsertCustomerRegistryAsync(registryDTO);

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
    public async Task<IEnumerable<string>> AddToCartAsync([FromBody] int? registryItemId, int? quantity)
    {
        if (registryItemId.IsNull())
        {
            return new string[] { "Unable to Add Item to Bag" };
        }

        return await _registryService.AddRegistryItemToCartAsync(registryItemId.Value, quantity);
    }
}