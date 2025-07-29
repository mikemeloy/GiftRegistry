using System.Collections.Generic;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;
using i7MEDIA.Plugin.Widgets.Registry.Models;

namespace i7MEDIA.Plugin.Widgets.Registry.Interfaces;

public interface IAdminService
{
    public Task UpsertConsultantAsync(RegistryConsultantDTO consultant);
    public Task<IEnumerable<RegistryConsultantDTO>> GetConsultantsAsync();
    public Task<RegistryList> QueryAsync(string query);
    public Task<IEnumerable<RegistryTypeDTO>> GetRegistryTypesAsync();
    public Task UpsertRegistryTypeAsync(RegistryTypeDTO registryType);
    public Task<IEnumerable<RegistryShippingOptionDTO>> GetShippingOptionsAsync();
    public Task UpsertRegistryShippingOptionAsync(RegistryShippingOptionDTO registryType);
    public Task UpdateAdminRegistryFields(RegistryEditAdminModel registry);
    public Task<RegistryEditAdminModel> GetRegistryByIdAsync(int id);
    public Task InsertExternalRegistryOrder(RegistryOrderDTO registryOrder);
    public Task DeleteExternalRegistryOrder(int orderId);
}