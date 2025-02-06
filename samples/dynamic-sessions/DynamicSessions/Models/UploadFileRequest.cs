namespace DynamicSessions.Models;

public record UploadFileRequest
{
    public required string Identifier { get; init; }

    public required string FileName { get; init; }

    public required Stream FileContent { get; init; }
}
