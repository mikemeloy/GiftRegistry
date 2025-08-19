using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace i7MEDIA.Plugin.Widgets.Registry.Models.Validation;

public class ValidationError
{
    [JsonPropertyName("message")]
    public string Message { get; set; }
    [JsonPropertyName("howToResolve")]
    public string HowToResolve { get; set; }
}

public class ValidationResponse
{
    [JsonPropertyName("errors")]
    public List<ValidationError> Errors { get; set; }
    [JsonPropertyName("isValid")]
    public bool IsValid { get; set; }
}