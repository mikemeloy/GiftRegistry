using System;
using System.Collections.Generic;
using System.Linq;
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
                email: consultant.Email,
                deleted: consultant.Deleted
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
            return await _registryRepository.GetRegistryConsultantsAsync();
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
            return await _registryRepository.GetRegistryTypesAsync();
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
                description: registryType.Description,
                deleted: registryType.Deleted
            );
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync(nameof(UpsertRegistryTypeAsync), e);
        }
    }

    public async Task<IEnumerable<RegistryShippingOptionDTO>> GetShippingOptionsAsync()
    {
        try
        {
            return await _registryRepository.GetRegistryShippingOptionsAsync();
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync(nameof(GetShippingOptionsAsync), e);
            return Enumerable.Empty<RegistryShippingOptionDTO>();
        }
    }

    public async Task UpsertRegistryShippingOptionAsync(RegistryShippingOptionDTO registryShippingOption)
    {
        if (registryShippingOption.IsNull())
        {
            return;
        }

        try
        {
            if (registryShippingOption.Id.IsNull())
            {
                await _registryRepository.InsertShippingOptionAsync(
                    name: registryShippingOption.Name,
                    description: registryShippingOption.Description
                );

                return;
            }

            await _registryRepository.UpdateShippingOptionAsync(
                id: registryShippingOption.Id,
                name: registryShippingOption.Name,
                description: registryShippingOption.Description,
                deleted: registryShippingOption.Deleted
            );
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync(nameof(UpsertRegistryShippingOptionAsync), e);
        }
    }

    public async Task UpdateAdminRegistryFields(AdminRegistryDTO registry)
    {
        try
        {
            await _registryRepository.UpdateRegistryAsync(registry);
        }
        catch (Exception e)
        {

            await _logger_R.LogErrorAsync(nameof(UpdateAdminRegistryFields), e);
        }
    }

    public async Task<RegistryEditAdminModel> GetRegistryByIdAsync(int id)
    {
        try
        {
            return await _registryRepository.GetAdminFieldsAsync(id);
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync(nameof(GetRegistryByIdAsync), e);
            return null;
        }
    }
}