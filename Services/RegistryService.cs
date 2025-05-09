using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Data;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;
using i7MEDIA.Plugin.Widgets.Registry.Extensions;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using i7MEDIA.Plugin.Widgets.Registry.Models;

namespace i7MEDIA.Plugin.Widgets.Registry.Services;

public class RegistryService : IRegistryService
{
    private readonly IRegistryRepository _registryRepository;
    private readonly ILogger_R _logger_R;
    public RegistryService(IRegistryRepository registryRepository, ILogger_R logger_R)
    {
        _registryRepository = registryRepository;
        _logger_R = logger_R;
    }

    public async Task<IEnumerable<GiftRegistry>> GetCurrentCustomerRegistriesAsync()
    {
        try
        {
            var registries = await _registryRepository.GetCurrentCustomerRegistriesAsync();

            return registries;
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync($"Unable to retrieve current users registries", e);
            return Enumerable.Empty<GiftRegistry>();
        }
    }

    public async Task<RegistryDTO> GetCustomerRegistryByIdAsync(int registryId)
    {
        try
        {
            var registry = await _registryRepository.GetRegistryByIdAsync(registryId);
            var items = await _registryRepository.GetRegistryItemsByIdAsync(registryId);
            var customer = _registryRepository.GetRegistryOwner(registryId);

            return registry
                .AddRegistryItems(items)
                .AddCustomerInfo(customer);
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync($"Unable to retrieve registry with the ID of {registryId}", e);
            return null;
        }
    }

    public async Task<bool> InsertCustomerRegistryAsync(string name, string description, DateTime eventDate)
    {
        try
        {
            await _registryRepository.InsertRegistryAsync(new RegistryDTO(name, description, eventDate));
            return true;
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync($"Unable to insert registry with the name of {name}", e);
            return false;
        }
    }

    public async Task<bool> InsertRegistryItemAsync(int registryId, int productId)
    {
        try
        {
            await _registryRepository.InsertRegistryItemAsync(registryId, productId);
            return true;
        }
        catch (Exception e)
        {

            await _logger_R.LogErrorAsync($"Unable to insert product with the id of {productId} int registry with the id of {registryId}", e);
            return false;
        }
    }

    public async Task<RegistryList> Query(string query)
    {
        try
        {
            var items = _registryRepository.Query(query);

            return new() { RegistryItems = items };
        }
        catch (Exception ex)
        {
            await _logger_R.LogErrorAsync($"Unable to perform request", ex);
            return new() { RegistryItems = Enumerable.Empty<RegistryListItem>() };
        }
    }
}