using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Data;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;
using i7MEDIA.Plugin.Widgets.Registry.Models;
using Nop.Core.Domain.Orders;

namespace i7MEDIA.Plugin.Widgets.Registry.Interfaces;

public interface IRegistryService
{
    public Task<IEnumerable<GiftRegistry>> GetCurrentCustomerRegistriesAsync();
    public Task<RegistryViewModel> GetCustomerRegistryByIdAsync(int registryId);
    public Task<bool> InsertCustomerRegistryAsync(RegistryDTO registryDTO);
    public Task<bool> InsertRegistryItemAsync(int registryId, int productId, int quantity);
    public Task<RegistryList> Query(string query);
    public Task<bool> DeleteRegistryAsync(int registryId);
    public Task<bool> DeleteRegistryItemAsync(int registryItemId);
    public Task<IEnumerable<string>> AddRegistryItemToCartAsync(int registryItemId, int? quantity);
    public Task ReconcileRegistry(Order order);
    public Task<bool> UpdateCustomerRegistryAsync(RegistryDTO registryDTO);
    public Task<bool> UpdateCustomerRegistryItemAsync(RegistryItemDTO registryItemDTO);
    public Task<IEnumerable<GiftReceiptOrderItem>> GetGiftReceiptOrderItemsAsync(int orderId);
    public Task<IList<RegistryListItem>> GetReportDataAsync(string name, DateTime start, DateTime end);
    Task GetRegistryItemsById(int registryId);
}