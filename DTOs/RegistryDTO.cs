using System;
using System.Collections.Generic;

namespace i7MEDIA.Plugin.Widgets.Registry.DTOs;

public record RegistryDTO(string Name, string Description, DateTime EventDate, string Notes, string Sponsor, int? Id = null, int EventType = 0, int ShippingOption = 0, int ConsultantId = 0, int CustomerId = 0)
{
    public string Owner { get; set; }
    public IList<RegistryItemDTO> RegistryItems { get; set; }
}