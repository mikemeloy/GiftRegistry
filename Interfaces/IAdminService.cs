using System.Collections.Generic;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;

namespace i7MEDIA.Plugin.Widgets.Registry.Interfaces;

public interface IAdminService
{
    public Task UpsertConsultantAsync(RegistryConsultantDTO consultant);
    public Task<IEnumerable<RegistryConsultantDTO>> GetConsultantsAsync();
}