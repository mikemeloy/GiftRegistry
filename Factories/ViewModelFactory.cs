using System;
using System.Linq;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Extensions;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using i7MEDIA.Plugin.Widgets.Registry.Models;
using i7MEDIA.Plugin.Widgets.Registry.Models.ViewModels;
using Nop.Core;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Plugins;

namespace i7MEDIA.Plugin.Widgets.Registry.Factories;

public class ViewModelFactory : IViewModelFactory
{
    private readonly IGenericAttributeService _genericAttributeService;
    private readonly IRegistryRepository _registryRepository;
    private readonly ICustomerService _customerService;
    private readonly IPluginService _pluginService;
    private readonly IAdminService _adminService;
    private readonly INopServices _nopServices;
    private readonly ILogger_R _logger_R;
    private readonly IStoreContext _storeContext;

    private string _version = null;

    public ViewModelFactory(IRegistryRepository registryRepository, IAdminService adminService, INopServices nopServices, ILogger_R logger_R, ICustomerService customerService, IGenericAttributeService genericAttributeService, IStoreContext storeContext, IPluginService pluginService)
    {
        _genericAttributeService = genericAttributeService;
        _registryRepository = registryRepository;
        _customerService = customerService;
        _pluginService = pluginService;
        _adminService = adminService;
        _storeContext = storeContext;
        _nopServices = nopServices;
        _logger_R = logger_R;
    }

    public async Task<ListViewModel> GetListViewModelAsync()
    {
        var customer = await _nopServices.GetCurrentCustomerAsync();
        var registryTypes = _registryRepository.GetRegistryTypesAsync();
        var shippingOptions = _registryRepository.GetRegistryShippingOptionsAsync();
        var isRegistered = _customerService.IsRegisteredAsync(customer);
        var version = await GetPluginVersionAsync();

        await Task.WhenAll(registryTypes, shippingOptions, isRegistered);

        return new(version, customer.FullName(), isRegistered.Result, registryTypes.Result, shippingOptions.Result);
    }

    public async Task<AdminViewModel> GetAdminViewModelAsync()
    {
        var version = await GetPluginVersionAsync();
        return new(version);
    }

    public async Task<RegistryPartialViewModel> GetRegistryPartialViewModelAsync()
    {
        var consultants = await _adminService.GetConsultantsAsync();
        var registryTypes = await _adminService.GetRegistryTypesAsync();
        var shippingOptions = await _adminService.GetShippingOptionsAsync();
        var version = await GetPluginVersionAsync();

        return new RegistryPartialViewModel(version, consultants, registryTypes, shippingOptions);
    }

    public async Task<ConsultantPartialViewModel> GetConsultantPartialViewModelAsync()
    {
        var consultants = await _adminService.GetConsultantsAsync();

        return new ConsultantPartialViewModel(consultants);
    }

    public async Task<RegistryTypePartialViewModel> GetRegistryTypePartialViewModelAsync()
    {
        var registryTypes = await _adminService.GetRegistryTypesAsync();

        return new RegistryTypePartialViewModel(registryTypes);
    }

    public async Task<RegistryShippingPartialViewModel> GetRegistryShippingOptionViewModelAsync()
    {
        var registryShippingOptions = await _adminService.GetShippingOptionsAsync();

        return new RegistryShippingPartialViewModel(registryShippingOptions);
    }

    public async Task<RegistryAdminRowViewModel> GetRegistryRowPartialViewModelAsync(string query)
    {
        if (query.IsNull())
        {
            return new(Enumerable.Empty<RegistryViewModel>());
        }
        try
        {
            var registryItems = await _registryRepository.AdminQueryAsync(query);

            return new(registryItems);
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync(nameof(GetRegistryRowPartialViewModelAsync), e);
        }

        return new(Enumerable.Empty<RegistryViewModel>());
    }

    public async Task<RegistryGiftReceiptViewModel> GetRegistryGiftReceiptViewModelAsync(string orderId)
    {
        var version = await GetPluginVersionAsync();
        _ = int.TryParse(orderId, out var id);

        return new RegistryGiftReceiptViewModel(
                FileName: "gift receipt.pdf",
                OrderId: id,
                PluginPath: RegistryDefaults.PluginPath,
                Version: version
            );
    }

    public async Task<RegistrySettingsViewModel> GetRegistrySettingsViewModel()
    {
        var store = await _storeContext.GetCurrentStoreAsync();
        var productKey = await _genericAttributeService.GetAttributeAsync<Guid?>(store, RegistryDefaults.ProductKeyAttribute, store.Id, null);

        return new(productKey);
    }

    private async Task<string> GetPluginVersionAsync()
    {
        if (_version.NotNull())
        {
            return _version;
        }

        var pluginDescriptor = await _pluginService.GetPluginDescriptorBySystemNameAsync<IPlugin>("i7MEDIA.Registry", LoadPluginsMode.InstalledOnly);

        _version = pluginDescriptor.Version;

        return _version;
    }
}