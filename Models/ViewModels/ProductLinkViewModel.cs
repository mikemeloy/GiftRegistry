using System.Collections.Generic;
using i7MEDIA.Plugin.Widgets.Registry.Data;

namespace i7MEDIA.Plugin.Widgets.Registry.Models.ViewModels;

public record ProductLinkViewModel(string PluginVersion, int ProductId)
{
    public IEnumerable<GiftRegistry> Registries { get; set; }
    public IEnumerable<RegistryProductAttribute> RequiredAttributes { get; set; }
}
