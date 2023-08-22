using System.Net;
using System.Text;
using AutoMapper;
using BookStoreAPI.Functions.Commands.Book.Create;
using BookStoreAPI.Functions.Commands.Book.Patch;
using BookStoreAPI.Models;
using BookStoreAPI.Tests.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace BookStoreAPI.Tests.IntegrationTests.Books;

public class BooksTests : IClassFixture<BookStoreWebApplicationFactory<Program>>
{
    private readonly BookStoreWebApplicationFactory<Program> _factory;
    private readonly IMapper _mapper;
    private readonly IServiceProvider _serviceProvider;

    public BooksTests(BookStoreWebApplicationFactory<Program> factory)
    {
        _factory = factory;

        _serviceProvider = _factory.Services;
        _mapper = _serviceProvider.GetRequiredService<IMapper>();

        Data.SeedData(_serviceProvider);
    }

    [Fact]
    public async Task Get_ReturnsListOfAllBooksFromDatabase()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BookStoreDatabaseContext>();

        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/BookStore");
        response.EnsureSuccessStatusCode();
        var contentString = await response.Content.ReadAsStringAsync();
        var books = JsonConvert.DeserializeObject<List<BookDto>>(contentString);

        var currentBooks = await dbContext.Books.Include(b => b.RentalHistory).Include(b => b.Genre).ToListAsync();
        var currentBooksDto = _mapper.Map<List<BookDto>>(currentBooks);

        Assert.NotNull(books);
        Assert.NotEmpty(books);
        Assert.Equivalent(currentBooksDto, books);
    }

    [Fact]
    public async Task Get_ReturnsBookFromDatabase()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BookStoreDatabaseContext>();

        var client = _factory.CreateClient();

        var firstBook = await dbContext.Books.FirstAsync();

        var response = await client.GetAsync($"api/BookStore/{firstBook.Id}");
        response.EnsureSuccessStatusCode();

        var contentString = await response.Content.ReadAsStringAsync();
        var book = JsonConvert.DeserializeObject<BookDto>(contentString);
        var firstBookDto = _mapper.Map<BookDto>(firstBook);

        Assert.NotNull(book);
        Assert.Equivalent(firstBookDto, book);
    }

    [Fact]
    public async Task Get_ReturnsNotFoundIfBookDoesntExist()
    {
        var client = _factory.CreateClient();

        var responseInvalid = await client.GetAsync("api/BookStore/11111");
        Assert.Equal(HttpStatusCode.NotFound, responseInvalid.StatusCode);
    }

    [Fact]
    public async Task Post_AddsBookToDatabase()
    {
        var client = _factory.CreateClient();

        var book = new CreateBookCommand
        {
            Title = "Dummy Title",
            Author = "Dummy Author",
            ReleaseDate = DateTime.Now,
            GenreId = 1
        };
        var bookSerialized = JsonConvert.SerializeObject(book);


        var response = await client.PostAsync("api/BookStore",
            new StringContent(bookSerialized, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Post_ReturnsErrorIfNonExistentBookGenre()
    {
        var client = _factory.CreateClient();

        var book = new CreateBookCommand
        {
            Title = "Dummy Title",
            Author = "Dummy Author",
            ReleaseDate = DateTime.Now,
            GenreId = 123
        };
        var bookSerialized = JsonConvert.SerializeObject(book);

        var response = await client.PostAsync("api/BookStore",
            new StringContent(bookSerialized, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task Put_UpdateBookInDatabase()
    {
        var client = _factory.CreateClient();

        var book = new BookDto
        {
            Title = "Dammy Title",
            Author = "Dammy Author",
            ReleaseDate = DateTime.Now,
            GenreId = 1
        };
        var bookSerialized = JsonConvert.SerializeObject(book);

        var response = await client.PutAsync("api/BookStore/1",
            new StringContent(bookSerialized, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Put_ReturnsErrorIfInvalidBookGenre()
    {
        var client = _factory.CreateClient();

        var book = new BookDto
        {
            Title = "Dammy Title",
            Author = "Dammy Author",
            ReleaseDate = DateTime.Now,
            GenreId = 123
        };
        var bookSerialized = JsonConvert.SerializeObject(book);

        var response = await client.PutAsync("api/BookStore/1",
            new StringContent(bookSerialized, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task Put_ReturnsNotFoundIfInvalidBookId()
    {
        var client = _factory.CreateClient();

        var book = new BookDto
        {
            Title = "Dammy Title",
            Author = "Dammy Author",
            ReleaseDate = DateTime.Now,
            GenreId = 123
        };
        var bookSerialized = JsonConvert.SerializeObject(book);

        var response = await client.PutAsync("api/BookStore/11111",
            new StringContent(bookSerialized, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_DeleteBookFromDatabase()
    {
        var client = _factory.CreateClient();

        var response = await client.DeleteAsync("api/BookStore/1");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnsNotFoundIfInvalidBookId()
    {
        var client = _factory.CreateClient();

        var response = await client.DeleteAsync("api/BookStore/11111");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Patch_UpdateBookInDatabase()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BookStoreDatabaseContext>();

        var client = _factory.CreateClient();

        var command = new PatchBookCommand
        {
            Title = "Diummy Book"
        };
        var commandSerialized = JsonConvert.SerializeObject(command);

        var response = await client.PatchAsync("api/BookStore/2",
            new StringContent(commandSerialized, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var book = await dbContext.Books.FirstOrDefaultAsync(b => b.Id == 2);
        Assert.Equal("Diummy Book", book.Title);
    }

    [Fact]
    public async Task Patch_ReturnsNotFoundIfInvalidBookId()
    {
        var client = _factory.CreateClient();

        var command = new PatchBookCommand
        {
            Title = "Diummy Book"
        };
        var commandSerialized = JsonConvert.SerializeObject(command);

        var response = await client.PatchAsync("api/BookStore/111111",
            new StringContent(commandSerialized, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}