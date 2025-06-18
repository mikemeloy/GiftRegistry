using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;
using i7MEDIA.Plugin.Widgets.Registry.Extensions;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using i7MEDIA.Plugin.Widgets.Registry.Models;

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

            await _registryRepository.UpdateConsultantAsync(
                id: consultant.Id,
                name: consultant.Name,
                email: consultant.Email
            );
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync(nameof(UpsertConsultantAsync), e);
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
            await _logger_R.LogErrorAsync(nameof(GetConsultantsAsync), e);
            return Enumerable.Empty<RegistryConsultantDTO>();
        }
    }

    public async Task<IEnumerable<RegistryTypeDTO>> GetRegistryTypesAsync()
    {
        try
        {
            return _registryRepository.GetRegistryTypes();
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync(nameof(GetRegistryTypesAsync), e);
            return Enumerable.Empty<RegistryTypeDTO>();
        }
    }

    public async Task<RegistryList> QueryAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return new() { RegistryItems = Enumerable.Empty<RegistryListItem>() };
        }

        try
        {
            var items = await _registryRepository.QueryAsync(query);

            return new() { RegistryItems = items };
        }
        catch (Exception ex)
        {
            await _logger_R.LogErrorAsync($"Unable to perform request", ex);
            return new() { RegistryItems = Enumerable.Empty<RegistryListItem>() };
        }
    }

    public async Task UpsertRegistryTypeAsync(RegistryTypeDTO registryType)
    {
        if (registryType.IsNull())
        {
            return;
        }

        try
        {
            if (registryType.Id.IsNull())
            {
                await _registryRepository.InsertRegistryTypeAsync(
                    name: registryType.Name,
                    description: registryType.Description
                );

                return;
            }

            await _registryRepository.UpdateRegistryTypeAsync(
              id: registryType.Id,
              name: registryType.Name,
              description: registryType.Description
          );
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync(nameof(UpsertRegistryTypeAsync), e);
        }
    }
}