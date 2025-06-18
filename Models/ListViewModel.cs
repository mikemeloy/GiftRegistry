using System.Collections.Generic;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;

namespace i7MEDIA.Plugin.Widgets.Registry.Models;

public record ListViewModel(string PluginVersion, List<RegistryTypeDTO> RegistryTypes);
