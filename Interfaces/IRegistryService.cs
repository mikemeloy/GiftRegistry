using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Data;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;
using i7MEDIA.Plugin.Widgets.Registry.Models;

namespace i7MEDIA.Plugin.Widgets.Registry.Interfaces;

public interface IRegistryService
{
    public Task<IEnumerable<GiftRegistry>> GetCurrentCustomerRegistriesAsync();
    public Task<RegistryViewModel> GetCustomerRegistryByIdAsync(int registryId);
    public Task<bool> InsertCustomerRegistryAsync(string name, string description, DateTime eventDate, string notes);
    public Task<bool> InsertRegistryItemAsync(int registryId, int productId, int quantity);
    public Task<RegistryList> Query(string query);
    public Task<bool> DeleteRegistryAsync(int registryId);
    public Task<bool> DeleteRegistryItemAsync(int registryItemId);
}