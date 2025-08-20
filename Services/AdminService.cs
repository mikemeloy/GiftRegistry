using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;
using i7MEDIA.Plugin.Widgets.Registry.Extensions;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using i7MEDIA.Plugin.Widgets.Registry.Models;
using i7MEDIA.Plugin.Widgets.Registry.Models.Validation;
using i7MEDIA.Plugin.Widgets.Registry.Settings;
using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Domain.Stores;
using Nop.Services.Common;
using Nop.Services.Plugins;

namespace i7MEDIA.Plugin.Widgets.Registry.Services;

public class AdminService : IAdminService
{
    private readonly ILogger_R _logger_R;
    private readonly IWebHelper _webHelper;
    private readonly INopServices _nopServices;
    private readonly IStoreContext _storeContext;
    private readonly IPluginService _pluginService;
    private readonly IHttpContextAccessor _httpContext;
    private readonly ISettingsService_R _settingsService_R;
    private readonly IRegistryRepository _registryRepository;
    private readonly IGenericAttributeService _genericAttributeService;

    public AdminService(IRegistryRepository registryRepository, ILogger_R logger_R, INopServices opServices, IHttpContextAccessor httpContext, IWebHelper webHelper, IGenericAttributeService genericAttributeService, IPluginService pluginService, IStoreContext storeContext, ISettingsService_R settingsService_R)
    {
        _logger_R = logger_R;
        _webHelper = webHelper;
        _nopServices = opServices;
        _httpContext = httpContext;
        _storeContext = storeContext;
        _pluginService = pluginService;
        _settingsService_R = settingsService_R;
        _registryRepository = registryRepository;
        _genericAttributeService = genericAttributeService;
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
                    email: consultant.Email,
                    sortOrder: consultant.SortOrder
                );

                return;
            }

            await _registryRepository.UpdateConsultantAsync(
                id: consultant.Id,
                name: consultant.Name,
                email: consultant.Email,
                sortOrder: consultant.SortOrder,
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
                    description: registryType.Description,
                    sortOrder: registryType.SortOrder
                );

                return;
            }

            await _registryRepository.UpdateRegistryTypeAsync(
                id: registryType.Id,
                name: registryType.Name,
                description: registryType.Description,
                sortOrder: registryType.SortOrder,
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
                    description: registryShippingOption.Description,
                    sortOrder: registryShippingOption.SortOrder
                );

                return;
            }

            await _registryRepository.UpdateShippingOptionAsync(
                id: registryShippingOption.Id,
                name: registryShippingOption.Name,
                description: registryShippingOption.Description,
                sortOrder: registryShippingOption.SortOrder,
                deleted: registryShippingOption.Deleted
            );
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync(nameof(UpsertRegistryShippingOptionAsync), e);
        }
    }

    public async Task UpdateAdminRegistryFields(RegistryEditAdminModel registry)
    {
        try
        {
            var (oldConsultant, newConsultant) = await _registryRepository.UpdateRegistryAsync(registry);

            await NotifyConsultantsOfChangeAsync(oldConsultant, newConsultant, registry);
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

    private async Task NotifyConsultantsOfChangeAsync(int? oldConsultant, int? newConsultant, RegistryEditAdminModel registry)
    {
        if (oldConsultant == newConsultant)
        {
            return;
        }

        var consultant = await _registryRepository.GetConsultantByIdAsync(newConsultant);

        if (consultant.IsNull())
        {
            await _logger_R.LogWarningAsync($"Unable to find a registry Consultant with the id of {newConsultant.Value}");
            return;
        }

        var request = _httpContext.HttpContext.Request;
        var domain = $"{request.Scheme}://{request.Host}";
        var registryPublicLink = $"<a href=\"{domain}/registry/{registry.Id}\" target=\"_blank\">{registry.Name}</a>";

        await _nopServices.SendRegistryConsultantEmailAsync(
                    subject: "You have been added to a Registry!",
                    body: $"Hello {consultant.Name},<br><br> You have been added as the consultant for {registryPublicLink}!",
                    consultant: consultant
                );

        var consultantOld = await _registryRepository.GetConsultantByIdAsync(oldConsultant);

        if (consultantOld.IsNull())
        {
            return;
        }

        await _nopServices.SendRegistryConsultantEmailAsync(
                    subject: $"You have been removed from {registry.Name}",
                    body: $"Hello {consultantOld.Name},<br><br>You have been removed as the consultant for {registryPublicLink}",
                    consultant: consultantOld
                );
    }

    public async Task InsertExternalRegistryOrder(RegistryOrderDTO registryOrder)
    {
        try
        {
            await _registryRepository.InsertExternalRegistryOrderAsync(registryOrder);
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync(nameof(InsertExternalRegistryOrder), e);
        }
    }

    public async Task DeleteExternalRegistryOrder(int orderId)
    {
        try
        {
            await _registryRepository.DeleteExternalRegistryOrderAsync(orderId);
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync(nameof(DeleteExternalRegistryOrder), e);
        }
    }

    public async Task<ValidationResponse> AddProductKeyAsync(Guid productKey)
    {
        return await ValidateProductKeyAsync(isNewProductKey: true, productKey);
    }

    public async Task<ValidationResponse> ValidateProductKeyAsync(bool isNewProductKey = false, Guid? newProductKey = null)
    {
        var store = await _storeContext.GetCurrentStoreAsync();
        var (isValid, expireDate, todayUTC) = await GetRegistryAttributesAsync(store);

        if (!isNewProductKey && isValid.NotNull() && (expireDate > todayUTC))
        {
            return GetResponse(
                isValid: isValid.Value
            );
        }

        var validationResponse = await CallProductKeyValidationServerAsync(newProductKey);

        if (!validationResponse.IsSuccessStatusCode)
        {
            return GetResponse(
                isValid: false,
                error: "Unable to Contact Product Key Server"
            );
        }

        var response = await ConvertResponseAsync(validationResponse);

        await SaveAttributesAsync(store, response.IsValid, newProductKey, todayUTC.AddDays(14));

        return response;
    }

    private static async Task<ValidationResponse> ConvertResponseAsync(HttpResponseMessage validationResponse)
    {
        return await JsonSerializer.DeserializeAsync<ValidationResponse>(validationResponse.Content.ReadAsStream());
    }

    private async Task<HttpResponseMessage> CallProductKeyValidationServerAsync(Guid? productKey)
    {
        var settings = await _settingsService_R.GetSettingsAsync<RegistrySettings>();
        var httpClient = new HttpClient();
        var secondaryIdentifier = _webHelper.GetStoreLocation();
        var featureId = await GetPluginMajorVersionAsync();
        var url = $"{settings.ProductKeyServerUrl}/License/ValidateLicense/{featureId}?licenseId={productKey}&secondaryIdentifier={secondaryIdentifier}";
        return await httpClient.PostAsync(url, null);
    }

    private static ValidationResponse GetResponse(bool isValid, string error = null)
    {
        return new ValidationResponse()
        {
            IsValid = isValid,
            Errors = error.IsNull() ? null : new() { new() { Message = error } }
        };
    }

    private async Task<(bool? IsValid, DateTime ExpireDate, DateTime TodayUTC)> GetRegistryAttributesAsync(Store store)
    {
        var todayUTC = DateTime.UtcNow;
        var isValid = await _genericAttributeService.GetAttributeAsync<bool?>(store, RegistryDefaults.ProductKeyValidAttribute, store.Id, null);
        var expiryDate = await _genericAttributeService.GetAttributeAsync(store, RegistryDefaults.ProductKeyExpireAttribute, store.Id, todayUTC);

        return (IsValid: isValid, ExpireDate: expiryDate, TodayUTC: todayUTC);
    }

    private async Task SaveAttributesAsync(Store store, bool isValid, Guid? productKey, DateTime expire)
    {
        await _genericAttributeService.SaveAttributeAsync(store, RegistryDefaults.ProductKeyValidAttribute, isValid, store.Id);
        await _genericAttributeService.SaveAttributeAsync(store, RegistryDefaults.ProductKeyAttribute, productKey, store.Id);
        await _genericAttributeService.SaveAttributeAsync(store, RegistryDefaults.ProductKeyExpireAttribute, expire, store.Id);
    }

    private async Task<string> GetPluginMajorVersionAsync()
    {
        try
        {
            var pluginDescriptor = await _pluginService.GetPluginDescriptorBySystemNameAsync<IPlugin>("i7MEDIA.Registry", LoadPluginsMode.InstalledOnly);

            return pluginDescriptor?.Version.Split('.').FirstOrDefault() ?? "1";

        }
        catch
        {
            return "1";
        }
    }
}