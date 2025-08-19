using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Models;
using i7MEDIA.Plugin.Widgets.Registry.Models.ViewModels;

namespace i7MEDIA.Plugin.Widgets.Registry.Interfaces;

public interface IViewModelFactory
{
    public Task<ListViewModel> GetListViewModelAsync();
    public AdminViewModel GetAdminViewModelAsync();
    public Task<RegistryPartialViewModel> GetRegistryPartialViewModelAsync();
    public Task<ConsultantPartialViewModel> GetConsultantPartialViewModelAsync();
    public Task<RegistryTypePartialViewModel> GetRegistryTypePartialViewModelAsync();
    public Task<RegistryShippingPartialViewModel> GetRegistryShippingOptionViewModelAsync();
    public Task<RegistryAdminRowViewModel> GetRegistryRowPartialViewModelAsync(string query);
    public RegistryGiftReceiptViewModel GetRegistryGiftReceiptViewModel(string orderId);
    public Task<RegistrySettingsViewModel> GetRegistrySettingsViewModel();
}