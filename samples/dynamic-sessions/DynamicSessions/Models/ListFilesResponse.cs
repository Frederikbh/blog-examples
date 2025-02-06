using System.Text.Json.Serialization;

namespace DynamicSessions.Models;

public record ListFilesResponse
{
    [JsonPropertyName("value")]
    public List<ListFilesResponseFile> Value { get; set; } = [];
}

public record ListFilesResponseFile
{
    [JsonPropertyName("properties")]
    public ListFilesResponseFileProperties? Properties { get; set; }
}

public record ListFilesResponseFileProperties
{
    [JsonPropertyName("filename")]
    public required string Filename { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("lastModifiedTime")]
    public DateTime? LastModifiedTime { get; set; }

    public string FullPath => $"/mnt/data/{Filename}";
}
