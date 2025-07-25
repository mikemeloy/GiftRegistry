using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Extensions;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using i7MEDIA.Plugin.Widgets.Registry.Models;

namespace i7MEDIA.Plugin.Widgets.Registry.Factories;

public class ViewModelFactory : IViewModelFactory
{
    private readonly IRegistryRepository _registryRepository;
    private readonly IAdminService _adminService;
    private readonly INopServices _nopServices;
    private readonly string _version = "1.0.2";

    public ViewModelFactory(IRegistryRepository registryRepository, IAdminService adminService, INopServices nopServices)
    {
        _registryRepository = registryRepository;
        _adminService = adminService;
        _nopServices = nopServices;
    }

    public async Task<ListViewModel> GetListViewModelAsync()
    {
        var customer = await _nopServices.GetCurrentCustomerAsync();
        var registryTypes = _registryRepository.GetRegistryTypesAsync();
        var shippingOptions = _registryRepository.GetRegistryShippingOptionsAsync();

        await Task.WhenAll(registryTypes, shippingOptions);

        return new("1.0.3", customer.FullName(), registryTypes.Result, shippingOptions.Result);
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
        var registryItems = await _registryRepository.AdminQueryAsync(query);

        return new RegistryAdminRowViewModel(registryItems);
    }

    public RegistryGiftReceiptViewModel GetRegistryGiftReceiptViewModel(string orderId)
    {
        _ = int.TryParse(orderId, out var id);

        return new RegistryGiftReceiptViewModel(
                FileName: "gift receipt.pdf",
                OrderId: id,
                PluginPath: RegistryDefaults.PluginPath,
                Version: "1.0.3"
            );
    }
}
