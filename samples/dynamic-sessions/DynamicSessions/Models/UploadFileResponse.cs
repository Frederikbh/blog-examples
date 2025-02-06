using System.Text.Json.Serialization;

namespace DynamicSessions.Models;

public record UploadFileResponse
{
    [JsonPropertyName("filename")]
    public string? Filename { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("lastModifiedTime")]
    public DateTime? LastModifiedTime { get; set; }

    public string FullPath => $"/mnt/data/{Filename}";
}
