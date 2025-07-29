namespace i7MEDIA.Plugin.Widgets.Registry.DTOs;

public record RegistryOrderDTO(string Notes, bool IsExternal, int Quantity, int RegistryItemId, bool IsDeleted);
