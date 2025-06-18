using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;
using i7MEDIA.Plugin.Widgets.Registry.Extensions;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;

namespace i7MEDIA.Plugin.Widgets.Registry.Services;

public class AdminService : IAdminService
{
    private readonly ILogger_R _logger_R;
    private readonly IRegistryRepository _registryRepository;

    public AdminService(IRegistryRepository registryRepository, ILogger_R logger_R)
    {
        _registryRepository = registryRepository;
        _logger_R = logger_R;
    }

    public async Task UpsertConsultantAsync(RegistryConsultantDTO consultant)
    {
        if (consultant.IsNull())
        {
            return;
        }

        try
        {
            if (consultant.Id.IsNull())
            {
                await _registryRepository.InsertConsultantAsync(
                    name: consultant.Name,
                    email: consultant.Email
                );

                return;
            }

            await _registryRepository.UpdateConsultantAsync(id: consultant.Id, name: consultant.Name, email: consultant.Email);
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync("Upsert Consultant", e);
        }
    }

    public async Task<IEnumerable<RegistryConsultantDTO>> GetConsultantsAsync()
    {
        try
        {
            return _registryRepository.GetRegistryConsultantsAsync();
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync("GetConsultant", e);
            return Enumerable.Empty<RegistryConsultantDTO>();
        }
    }
}