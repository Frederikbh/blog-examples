using System.Text.Json.Serialization;

namespace DynamicSessions.Models;

public record CodeExecutionResponse
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("result")]
    public object? Result { get; set; }

    [JsonPropertyName("stdout")]
    public object? Stdout { get; set; }

    [JsonPropertyName("stderr")]
    public object? Stderr { get; set; }

    public string Format() =>
        $"""
         Status: 
         {Status}
         Result:
         {Result}
         Stdout:
         {Stdout}
         Stderr:
         {Stderr}
         """;
}
