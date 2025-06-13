using i7MEDIA.Plugin.Widgets.Registry.Models;

namespace i7MEDIA.Plugin.Widgets.Registry.Interfaces;

public interface IViewModelFactory
{
    public ListViewModel GetListViewModelAsync();
    public AdminViewModel GetAdminViewModelAsync();
}