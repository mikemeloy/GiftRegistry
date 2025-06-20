using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Models;

namespace i7MEDIA.Plugin.Widgets.Registry.Interfaces;

public interface IViewModelFactory
{
    public ListViewModel GetListViewModelAsync();
    public AdminViewModel GetAdminViewModelAsync();
    public RegistryPartialViewModel GetRegistryPartialViewModelAsync();
    public Task<ConsultantPartialViewModel> GetConsultantPartialViewModelAsync();
    public Task<RegistryTypePartialViewModel> GetRegistryTypePartialViewModelAsync();
    public Task<RegistryShippingPartialViewModel> GetRegistryShippingOptionViewModelAsync();
}