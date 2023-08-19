using BookStoreAPI.Models;
using Microsoft.Extensions.DependencyInjection;

namespace BookStoreAPI.Tests.Utils;

public class Data
{
    public static void SeedData(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BookStoreDatabaseContext>();

        dbContext.Database.EnsureCreated();

        var genre = new Genre
        {
            Name = "DummyGenre"
        };
        dbContext.Genres.Add(genre);


        dbContext.SaveChanges();

        var genreId = genre.Id;

        dbContext.Books.AddRange(new List<Book>
        {
            new()
            {
                Title = "Test Book 1",
                Author = "DummyAuthor1",
                ReleaseDate = DateTime.Now,
                GenreId = genreId
            },
            new()
            {
                Title = "Test Book 2",
                Author = "DummyAuthor2",
                ReleaseDate = DateTime.Now,
                GenreId = genreId
            }
        });


        dbContext.Clients.AddRange(new List<Client>
        {
            new()
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = DateTime.Now
            },
            new()
            {
                FirstName = "Joe",
                LastName = "Doe",
                DateOfBirth = DateTime.Now
            }
        });

        dbContext.SaveChanges();
    }
}