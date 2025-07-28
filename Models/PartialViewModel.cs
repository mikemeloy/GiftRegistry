using System.Collections.Generic;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;

namespace i7MEDIA.Plugin.Widgets.Registry.Models;

public record RegistryPartialViewModel(
                    string PluginVersion,
                    IEnumerable<RegistryConsultantDTO> Consultants,
                    IEnumerable<RegistryTypeDTO> RegistryTypes,
                    IEnumerable<RegistryShippingOptionDTO> ShippingOptions
                );

public record ConsultantPartialViewModel(IEnumerable<RegistryConsultantDTO> Consultants);

public record RegistryTypePartialViewModel(IEnumerable<RegistryTypeDTO> RegistryTypes);