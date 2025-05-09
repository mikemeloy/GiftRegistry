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
    public Task<RegistryDTO> GetCustomerRegistryByIdAsync(int registryId);
    public Task<bool> InsertCustomerRegistryAsync(string name, string description, DateTime eventDate);
    public Task<bool> InsertRegistryItemAsync(int registryId, int productId);
    public Task<RegistryList> Query(string query);
}