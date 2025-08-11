namespace i7MEDIA.Plugin.Widgets.Registry.DTOs;

public record RegistryConsultantDTO(int? Id, string Name, string Email, int SortOrder, bool Deleted = false);