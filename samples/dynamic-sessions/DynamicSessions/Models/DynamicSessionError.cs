namespace DynamicSessions.Models;

public record DynamicSessionError
{
    public required string Message { get; set; }

    public required string Code { get; set; }
}
