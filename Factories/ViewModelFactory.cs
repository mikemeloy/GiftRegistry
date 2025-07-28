using System;
using System.Linq;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Extensions;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using i7MEDIA.Plugin.Widgets.Registry.Models;
using Nop.Services.Customers;

namespace i7MEDIA.Plugin.Widgets.Registry.Factories;

public class ViewModelFactory : IViewModelFactory
{
    private readonly IRegistryRepository _registryRepository;
    private readonly IAdminService _adminService;
    private readonly ICustomerService _customerService;
    private readonly INopServices _nopServices;
    private readonly ILogger_R _logger_R;
    private readonly string _version = "1.0.3";

    public ViewModelFactory(IRegistryRepository registryRepository, IAdminService adminService, INopServices nopServices, ILogger_R logger_R, ICustomerService customerService)
    {
        _registryRepository = registryRepository;
        _customerService = customerService;
        _adminService = adminService;
        _nopServices = nopServices;
        _logger_R = logger_R;
    }

    public async Task<ListViewModel> GetListViewModelAsync()
    {
        var customer = await _nopServices.GetCurrentCustomerAsync();
        var registryTypes = _registryRepository.GetRegistryTypesAsync();
        var shippingOptions = _registryRepository.GetRegistryShippingOptionsAsync();
        var isRegistered = _customerService.IsRegisteredAsync(customer);

        await Task.WhenAll(registryTypes, shippingOptions, isRegistered);

        return new(_version, customer.FullName(), isRegistered.Result, registryTypes.Result, shippingOptions.Result);
    }

    public AdminViewModel GetAdminViewModelAsync()
    {
        return new(_version);
    }

    public async Task<RegistryPartialViewModel> GetRegistryPartialViewModelAsync()
    {
        var consultants = await _adminService.GetConsultantsAsync();
        var registryTypes = await _adminService.GetRegistryTypesAsync();
        var shippingOptions = await _adminService.GetShippingOptionsAsync();

        return new RegistryPartialViewModel(_version, consultants, registryTypes, shippingOptions);
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

    public RegistryGiftReceiptViewModel GetRegistryGiftReceiptViewModel(string orderId)
    {
        _ = int.TryParse(orderId, out var id);

        return new RegistryGiftReceiptViewModel(
                FileName: "gift receipt.pdf",
                OrderId: id,
                PluginPath: RegistryDefaults.PluginPath,
                Version: _version
            );
    }
}
