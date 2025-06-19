using System.Collections.Generic;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;

namespace i7MEDIA.Plugin.Widgets.Registry.Models;

public record RegistryPartialViewModel();

public record ConsultantPartialViewModel(IEnumerable<RegistryConsultantDTO> Consultants);

public record RegistryTypePartialViewModel(IEnumerable<RegistryTypeDTO> RegistryTypes);