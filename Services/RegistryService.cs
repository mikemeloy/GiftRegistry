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
            var (iAmOwner, registryOwnerName) = await _registryRepository.GetRegistryOwnerAssociationAsync(registryId);
            var consultant = await _registryRepository.GetConsultantByIdAsync(registry.GetValueOrDefault(r => r.ConsultantId));

            foreach (var item in items)
            {
                item.ImageUrl = await _nopServices.GetProductImageUrlAsync(item.ProductId);
            }

            return new RegistryViewModel()
            {
                Id = registryId,
                Name = registry.GetValueOrDefault(r => r.Name),
                OwnerName = registryOwnerName,
                Description = registry.GetValueOrDefault(r => r.Description),
                EventDate = registry.GetValueOrDefault(r => r.EventDate),
                EventType = registry.GetValueOrDefault(r => r.EventType),
                Sponsor = registry.GetValueOrDefault(r => r.Sponsor),
                Notes = registry.GetValueOrDefault(r => r.Notes),
                ShippingOption = registry.GetValueOrDefault(r => r.ShippingOption),
                RegistryItems = items.SortRegistryItems(),
                IamOwner = iAmOwner,
                ConsultantName = consultant.GetValueOrDefault(c => c.Name),
                ConsultantEmail = consultant.GetValueOrDefault(c => c.Email)
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

    public async Task<IEnumerable<string>> AddRegistryItemToCartAsync(int registryItemId, int? quantity)
    {
        try
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
                    quantity: quantity ?? 1
                );

            if (addToCartWarnings.Any())
            {
                return addToCartWarnings;
            }

            await _nopServices.AddRegistryItemAttributeAsync(customer, registryItemId);
            return Enumerable.Empty<string>();

        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync(nameof(AddRegistryItemToCartAsync), e);
            return Enumerable.Empty<string>();
        }
    }

    public async Task ReconcileRegistry(Order order)
    {
        try
        {
            var customer = await _nopServices.GetCustomerByIdAsync(order.CustomerId);
            var registryItemsInCart = await _nopServices.GetRegistryItemAttributeAsync(customer);

            if (registryItemsInCart.Length == 0)
            {
                return;
            }

            var orderItems = await _orderService.GetOrderItemsAsync(order.Id);

            foreach (var registryItemId in registryItemsInCart)
            {
                var registryItem = await _registryRepository.GetRegistryItemByIdAsync(registryItemId);

                if (registryItem.IsNull())
                {
                    continue;
                }

                var orderItem = orderItems.FirstOrDefault(oi => oi.ProductId == registryItem.ProductId);

                if (orderItem.IsNull())
                {
                    continue;
                }

                var registryInfo = await GetRegistryDataByRegistryItemIdAsync(registryItem, order.Id);

                await _registryRepository.InsertRegistryItemOrderAsync(
                    orderId: order.Id,
                    productId: registryItem.Id,
                    registryId: registryItem.RegistryId,
                    quantity: orderItem.Quantity
                );

                if (registryInfo.IsNull())
                {
                    continue;
                }

                await _nopServices.InsertOrderNoteAsync(
                    orderId: registryInfo.OrderId,
                    note: registryInfo.GetRegistryAdminNote()
                );

                await _nopServices.SendRegistryConsultantEmailAsync(
                    subject: registryInfo.GetRegistryOrderEmailSubject(),
                    body: registryInfo.GetRegistryOrderEmailBody(),
                    consultant: registryInfo.Consultant
                );
            }

            await _nopServices.ClearRegistryItemAttributeAsync(customer);
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync(nameof(ReconcileRegistry), e);
        }
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
            await _logger_R.LogErrorAsync(nameof(UpdateCustomerRegistryAsync), e);
            return false;
        }
    }

    public async Task<bool> UpdateCustomerRegistryItemAsync(RegistryItemDTO registryItemDTO)
    {
        try
        {
            await _registryRepository.UpdateRegistryItemAsync(registryItemDTO);
            return true;
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync(nameof(UpdateCustomerRegistryItemAsync), e);
            return false;
        }
    }

    public async Task<IEnumerable<GiftReceiptOrderItem>> GetGiftReceiptOrderItemsAsync(int orderId)
    {
        try
        {
            return await _registryRepository.GetGiftReceiptOrderItemsAsync(orderId);
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync(nameof(GetGiftReceiptOrderItemsAsync), e);
        }

        return Enumerable.Empty<GiftReceiptOrderItem>();
    }

    private async Task<RegistryData> GetRegistryDataByRegistryItemIdAsync(GiftRegistryItem registryItem, int orderId)
    {
        try
        {
            var registry = await _registryRepository.GetRegistryByIdAsync(registryItem.RegistryId);
            var product = await _nopServices.GetProductByIdAsync(registryItem.ProductId);
            var consultant = await _registryRepository.GetConsultantByIdAsync(registry.ConsultantId);

            return new(registry.Name, product.Name, consultant, orderId);
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync(nameof(GetRegistryDataByRegistryItemIdAsync), e);
            return new(string.Empty, string.Empty, new(), 0);
        }
    }

    public async Task<IEnumerable<RegistryListItem>> GetReportDataAsync(string name, DateTime start, DateTime end)
    {
        try
        {
            return await _registryRepository.QueryAsync(name, start, end);
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync(nameof(GetReportDataAsync), e);
            return Enumerable.Empty<RegistryListItem>();
        }
    }

    public async Task<IEnumerable<RegistryItemViewModel>> GetRegistryItemsByIdAsync(int registryId)
    {
        try
        {
            return await _registryRepository.GetRegistryItemsByIdAsync(registryId);
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync(nameof(GetRegistryItemsByIdAsync), e);
            return Enumerable.Empty<RegistryItemViewModel>();
        }
    }

    public async Task<IEnumerable<RegistryOrderViewModel>> GetRegistryOrdersByIdAsync(int registryId)
    {
        try
        {
            return await _registryRepository.GetRegistryOrdersByIdAsync(registryId);
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync(nameof(GetRegistryOrdersByIdAsync), e);
            return Enumerable.Empty<RegistryOrderViewModel>();
        }
    }
}