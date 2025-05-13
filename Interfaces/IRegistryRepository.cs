using System.Collections.Generic;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Data;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;
using i7MEDIA.Plugin.Widgets.Registry.Models;

namespace i7MEDIA.Plugin.Widgets.Registry.Interfaces;

public interface IRegistryRepository
{
    public Task<RegistryDTO> GetRegistryByIdAsync(int registryId);
    public Task<bool> GetRegistryOwnerAssociationAsync(int registryId);
    public Task<List<RegistryItemViewModel>> GetRegistryItemsByIdAsync(int registryId);
    public Task InsertRegistryAsync(RegistryDTO registry);
    public IList<RegistryListItem> Query(string query);
    public Task InsertRegistryItemAsync(int registryId, int productId);
    public Task DeleteRegistryAsync(int registryId);
    public Task DeleteRegistryItemAsync(int registryItemId);
    public Task<IEnumerable<GiftRegistry>> GetCurrentCustomerRegistriesAsync();
}