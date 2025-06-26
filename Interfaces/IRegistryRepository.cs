using System.Collections.Generic;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Data;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;
using i7MEDIA.Plugin.Widgets.Registry.Models;

namespace i7MEDIA.Plugin.Widgets.Registry.Interfaces;

public interface IRegistryRepository
{
    public Task<IList<RegistryViewModel>> AdminQueryAsync(string q);
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
    public Task UpdateRegistryAsync(RegistryDTO registryDTO);
    public Task<IEnumerable<RegistryTypeDTO>> GetRegistryTypesAsync();
    public Task InsertRegistryItemOrderAsync(int orderId, int registryId, int productId, int quantity);
    public Task InsertConsultantAsync(string name, string email);
    public Task UpdateConsultantAsync(int? id, string name, string email, bool deleted = false);
    public Task<IEnumerable<RegistryConsultantDTO>> GetRegistryConsultantsAsync();
    public Task InsertRegistryTypeAsync(string name, string description);
    public Task UpdateRegistryTypeAsync(int? id, string name, string description, bool deleted = false);
    public Task InsertShippingOptionAsync(string name, string description);
    public Task UpdateShippingOptionAsync(int? id, string name, string description, bool deleted);
    public Task<IEnumerable<RegistryShippingOptionDTO>> GetRegistryShippingOptionsAsync();
    public Task<GiftRegistryConsultant> GetConsultantByIdAsync(int consultantId);
}