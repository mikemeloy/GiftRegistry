using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Data;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;
using i7MEDIA.Plugin.Widgets.Registry.Extensions;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using i7MEDIA.Plugin.Widgets.Registry.Models;
using Nop.Core.Domain.Orders;
using Nop.Services.Orders;

namespace i7MEDIA.Plugin.Widgets.Registry.Services;

public class RegistryService : IRegistryService
{
    private readonly ILogger_R _logger_R;
    private readonly INopServices _nopServices;
    private readonly IRegistryRepository _registryRepository;
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IOrderService _orderService;
    public RegistryService(IOrderService orderService, IShoppingCartService shoppingCartService, IRegistryRepository registryRepository, INopServices opServices, ILogger_R logger_R)
    {
        _shoppingCartService = shoppingCartService;
        _registryRepository = registryRepository;
        _orderService = orderService;
        _nopServices = opServices;
        _logger_R = logger_R;
    }

    public async Task<bool> DeleteRegistryAsync(int id)
    {
        try
        {
            await _registryRepository.DeleteRegistryAsync(id);
            return true;
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync($"Unable to delete registry with the id of {id}", e);
            return false;
        }
    }

    public async Task<bool> DeleteRegistryItemAsync(int registryItemId)
    {
        try
        {
            await _registryRepository.DeleteRegistryItemAsync(registryItemId);
            return true;
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync($"Unable to delete registry with the id of {registryItemId}", e);
            return false;
        }
    }

    public async Task<IEnumerable<GiftRegistry>> GetCurrentCustomerRegistriesAsync()
    {
        try
        {
            var registries = await _registryRepository.GetCurrentCustomerRegistriesAsync();

            return registries;
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync($"Unable to retrieve current users registries", e);
            return Enumerable.Empty<GiftRegistry>();
        }
    }

    public async Task<RegistryViewModel> GetCustomerRegistryByIdAsync(int registryId)
    {
        try
        {
            var registry = await _registryRepository.GetRegistryByIdAsync(registryId);
            var items = await _registryRepository.GetRegistryItemsByIdAsync(registryId);
            var iAmOwner = await _registryRepository.GetRegistryOwnerAssociationAsync(registryId);

            foreach (var item in items)
            {
                item.ImageUrl = await _nopServices.GetProductImageUrlAsync(item.ProductId);
            }

            return new RegistryViewModel()
            {
                Id = registryId,
                Name = registry.GetValueOrDefault(r => r.Name),
                Description = registry.GetValueOrDefault(r => r.Description),
                EventDate = registry.GetValueOrDefault(r => r.EventDate),
                EventType = registry.GetValueOrDefault(r => r.EventType),
                RegistryItems = items,
                IamOwner = iAmOwner,
                Notes = registry.GetValueOrDefault(r => r.Notes)
            };
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync($"Unable to retrieve registry with the ID of {registryId}", e);
            return null;
        }
    }

    public async Task<bool> InsertCustomerRegistryAsync(RegistryDTO registryDTO)
    {
        try
        {
            await _registryRepository.InsertRegistryAsync(registryDTO);
            return true;
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync($"Unable to insert registry with the name of {registryDTO.Name}", e);
            return false;
        }
    }

    public async Task<bool> InsertRegistryItemAsync(int registryId, int productId, int quantity)
    {
        try
        {
            await _registryRepository.InsertRegistryItemAsync(registryId, productId, quantity);
            return true;
        }
        catch (Exception e)
        {

            await _logger_R.LogErrorAsync($"Unable to insert product with the id of {productId} int registry with the id of {registryId}", e);
            return false;
        }
    }

    public async Task<RegistryList> Query(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return new() { RegistryItems = Enumerable.Empty<RegistryListItem>() };
        }

        try
        {
            var items = await _registryRepository.QueryAsync(query);

            return new() { RegistryItems = items };
        }
        catch (Exception ex)
        {
            await _logger_R.LogErrorAsync($"Unable to perform request", ex);
            return new() { RegistryItems = Enumerable.Empty<RegistryListItem>() };
        }
    }

    public async Task<IEnumerable<string>> AddRegistryItemToCartAsync(int registryItemId)
    {
        var customer = await _nopServices.GetCurrentCustomerAsync();
        var store = await _nopServices.GetStoreAsync();
        var item = await _registryRepository.GetRegistryItemByIdAsync(registryItemId);
        var product = await _nopServices.GetProductByIdAsync(item.ProductId);

        var addToCartWarnings = await _shoppingCartService.AddToCartAsync(
                customer: customer,
                product: product,
                shoppingCartType: ShoppingCartType.ShoppingCart,
                storeId: store.Id,
                attributesXml: null, //TODO:will need for complex products 
                quantity: item.Quantity
            );

        if (addToCartWarnings.Any())
        {
            return addToCartWarnings;
        }

        await _nopServices.AddRegistryItemAttributeAsync(customer, registryItemId);
        return Enumerable.Empty<string>();
    }
    /// <summary>
    /// Need to test concurrency!!
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    public async Task ReconcileRegistry(Order order)
    {
        //1. get customer.
        var customer = await _nopServices.GetCustomerByIdAsync(order.CustomerId);
        //2. get attribute.
        var registryItemIds = await _nopServices.GetRegistryItemAttributeAsync(customer);
        //if not return early...
        if (registryItemIds.Length == 0)
        {
            return;
        }
        //3. compare order with attribute.
        var orderItems = await _orderService.GetOrderItemsAsync(order.Id);

        foreach (var registryItemId in registryItemIds)
        {
            var regItem = await _registryRepository.GetRegistryItemByIdAsync(registryItemId);

            if (regItem.IsNull())
            {
                continue;
            }

            var hasProduct = orderItems.Any(oi => oi.ProductId == regItem.ProductId);

            if (!hasProduct)
            {
                continue;
            }

            //4. mark items on registry as purchase by adding order id to registry item :)
            regItem.OrderId = order.Id;
            await _registryRepository.UpdateRegistryItemAsync(regItem);
        }

        //5. clear registry attribute (if they didn't purchase them this time...)
        await _nopServices.ClearRegistryItemAttributeAsync(customer);
    }

    public async Task<bool> UpdateCustomerRegistryAsync(RegistryDTO registryDTO)
    {
        try
        {
            await _registryRepository.UpdateRegistryAsync(registryDTO);
            return true;
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync($"Unable to update registry with the id of {registryDTO.Id}", e);
            return false;
        }
    }
}