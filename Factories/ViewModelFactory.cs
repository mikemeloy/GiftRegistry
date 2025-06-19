using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using i7MEDIA.Plugin.Widgets.Registry.Models;

namespace i7MEDIA.Plugin.Widgets.Registry.Factories;

public class ViewModelFactory : IViewModelFactory
{
    private readonly IRegistryRepository _registryRepository;
    private readonly IAdminService _adminService;

    public ViewModelFactory(IRegistryRepository registryRepository, IAdminService adminService)
    {
        _registryRepository = registryRepository;
        _adminService = adminService;
    }

    public ListViewModel GetListViewModelAsync()
    {
        var registryTypes = _registryRepository.GetRegistryTypes();

        return new("1.0.1", registryTypes);
    }

    public AdminViewModel GetAdminViewModelAsync()
    {
        return new("1.0.1");
    }

    public RegistryPartialViewModel GetRegistryPartialViewModelAsync()
    {
        return new RegistryPartialViewModel();
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
}
