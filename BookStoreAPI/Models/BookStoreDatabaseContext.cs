using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Models;

public class BookStoreDatabaseContext : DbContext
{
    public BookStoreDatabaseContext(DbContextOptions<BookStoreDatabaseContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Client> Clients { get; set; }
}