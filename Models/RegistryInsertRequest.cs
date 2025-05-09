using System;

namespace i7MEDIA.Plugin.Widgets.Registry.Models;

public record RegistryInsertRequest(string Name, string Description, DateTime EventDate);