using System.Collections.Generic;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;

namespace i7MEDIA.Plugin.Widgets.Registry.Models.ViewModels;

public record RegistryShippingPartialViewModel(IEnumerable<RegistryShippingOptionDTO> RegistryShippingOptions);