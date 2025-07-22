using System;

namespace i7MEDIA.Plugin.Widgets.Registry.DTOs;

public record ReportRequestDTO(string Name, bool? Status, DateTime StartDate, DateTime EndDate);
