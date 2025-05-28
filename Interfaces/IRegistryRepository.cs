using System.Collections.Generic;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Data;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;
using i7MEDIA.Plugin.Widgets.Registry.Models;

namespace i7MEDIA.Plugin.Widgets.Registry.Interfaces;

public interface IRegistryRepository
{
    public Task<IList<RegistryListItem>> QueryAsync(string query);
    public Task<RegistryDTO> GetRegistryByIdAsync(int registryId);
    public Task InsertRegistryAsync(RegistryDTO registry);
    public Task DeleteRegistryAsync(int registryId);
    public Task<List<RegistryItemViewModel>> GetRegistryItemsByIdAsync(int registryId);
    public Task<GiftRegistryItem> GetRegistryItemByIdAsync(int registryItemId);
    public Task InsertRegistryItemAsync(int registryId, int productId, int quantity);
    public Task UpdateRegistryItemAsync(GiftRegistryItem item);
    public Task DeleteRegistryItemAsync(int registryItemId);
    public Task<bool> GetRegistryOwnerAssociationAsync(int registryId);
    public Task<IEnumerable<GiftRegistry>> GetCurrentCustomerRegistriesAsync();
}