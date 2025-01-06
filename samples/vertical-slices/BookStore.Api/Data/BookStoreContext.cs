using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Data;

public class BookStoreContext : DbContext
{
    public DbSet<Book> Books { get; internal set; } = null!;

    public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().HasKey(e => e.Id);
    }

    internal static void Seed(DbContext context, bool _)
    {
        AddIfNotExists(context, new Book
        {
            Id = "100",
            BookName = "The Fellowship of the Ring",
            Author = "J.R.R. Tolkien",
            PublishedAt = new DateTime(1954, 7, 29)
        });

        AddIfNotExists(context, new Book
        {
            Id = "101",
            BookName = "The Two Towers",
            Author = "J.R.R. Tolkien",
            PublishedAt = new DateTime(1954, 11, 11)
        });

        context.SaveChanges();
    }

    private static void AddIfNotExists(DbContext context, Book book)
    {
        if (context.Set<Book>().Find(book.Id) is null)
        {
            context.Set<Book>().Add(book);
        }
    }
}

