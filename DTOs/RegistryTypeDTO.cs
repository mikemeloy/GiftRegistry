namespace i7MEDIA.Plugin.Widgets.Registry.DTOs;

public record RegistryTypeDTO(int? Id, string Name, string Description, int SortOrder, bool Deleted = false);
