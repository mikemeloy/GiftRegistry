using System.Collections.Generic;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Data;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;
using i7MEDIA.Plugin.Widgets.Registry.Models;
using Nop.Core.Domain.Customers;

namespace i7MEDIA.Plugin.Widgets.Registry.Interfaces;

public interface IRegistryRepository
{
    public Task<RegistryDTO> GetRegistryByIdAsync(int registryId);
    public Customer GetRegistryOwner(int customerId);
    public Task<IList<RegistryItemDTO>> GetRegistryItemsByIdAsync(int registryId);
    public Task InsertRegistryAsync(RegistryDTO registry);
    public IList<RegistryListItem> Query(string query);
    public Task InsertRegistryItemAsync(int registryId, int productId);
    public Task<IEnumerable<GiftRegistry>> GetCurrentCustomerRegistriesAsync();
}