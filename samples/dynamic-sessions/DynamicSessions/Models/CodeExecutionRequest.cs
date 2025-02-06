using System.Text.Json.Serialization;

namespace DynamicSessions.Models;

public record CodeExecutionRequest
{
    [JsonPropertyName("identifier")]
    public required string Identifier { get; set; }

    [JsonPropertyName("pythonCode")]
    public required string PythonCode { get; set; }

    [JsonPropertyName("timeoutInSeconds")]
    public int TimeoutInSeconds { get; set; } = 100;

    [JsonPropertyName("codeInputType")]
    public CodeInputTypeSetting CodeInputType { get; set; } = CodeInputTypeSetting.Inline;

    [JsonPropertyName("executionType")]
    public CodeExecutionTypeSetting CodeExecutionType { get; set; } = CodeExecutionTypeSetting.Synchronous;
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CodeInputTypeSetting
{
    [JsonPropertyName("inline")]
    Inline
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CodeExecutionTypeSetting
{
    [JsonPropertyName("synchronous")]
    Synchronous
}
