using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using i7MEDIA.Plugin.Widgets.Registry.Models;

namespace i7MEDIA.Plugin.Widgets.Registry.Factories;

public class ViewModelFactory : IViewModelFactory
{
    private readonly IRegistryRepository _registryRepository;

    public ViewModelFactory(IRegistryRepository registryRepository)
    {
        _registryRepository = registryRepository;
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
}
