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
    Task<IEnumerable<RegistryShippingOptionDTO>> GetShippingOptionsAsync();
    Task UpsertRegistryShippingOptionAsync(RegistryShippingOptionDTO registryType);
}