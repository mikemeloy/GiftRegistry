using System.Collections.Generic;

namespace i7MEDIA.Plugin.Widgets.Registry.Models;

public record ListViewModel(string PluginVersion, List<RegistryTypeDTO> RegistryTypes);
