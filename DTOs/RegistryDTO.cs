using System;
using System.Collections.Generic;

namespace i7MEDIA.Plugin.Widgets.Registry.DTOs;

public record RegistryDTO(string Name, string Description, DateTime EventDate, string Notes, int? Id = null, int EventType = 0)
{
    public string Owner { get; set; }
    public IList<RegistryItemDTO> RegistryItems { get; set; }
}

public record RegistryItemDTO(int Id, int ProductId, int CartItemId);