namespace BookStore.Api.Data;

public record Book
{
    public required string Id { get; init; }

    public required string BookName { get; set; }

    public required string Author { get; set; }

    public required DateTime PublishedAt { get; set; }
}
