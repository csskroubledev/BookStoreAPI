using System.Net;
using System.Text;
using AutoMapper;
using BookStoreAPI.Functions.Commands.Book.Create;
using BookStoreAPI.Functions.Commands.Book.Patch;
using BookStoreAPI.Models;
using BookStoreAPI.Tests.Utils;
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
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/BookStore");
        response.EnsureSuccessStatusCode();
        var contentString = await response.Content.ReadAsStringAsync();
        var books = JsonConvert.DeserializeObject<IEnumerable<BookDto>>(contentString);

        Assert.NotNull(books);
        Assert.NotEmpty(books);
    }

    [Fact]
    public async Task Get_ReturnsBookFromDatabase()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BookStoreDatabaseContext>();

        var client = _factory.CreateClient();

        var firstBook = dbContext.Books.First();

        var response = await client.GetAsync($"api/BookStore/{firstBook.Id}");
        response.EnsureSuccessStatusCode();

        var contentString = await response.Content.ReadAsStringAsync();
        var book = JsonConvert.DeserializeObject<BookDto>(contentString);
        var firstBookDto = _mapper.Map<BookDto>(firstBook);

        Assert.NotNull(book);
        Assert.Equivalent(firstBookDto, book);

        var responseInvalid = await client.GetAsync("api/BookStore/11111");
        Assert.Equal(HttpStatusCode.NotFound, responseInvalid.StatusCode);
    }

    [Fact]
    public async Task Post_AddsBookToDatabase()
    {
        var client = _factory.CreateClient();

        var correctBook = new CreateBookCommand
        {
            Title = "Dummy Title",
            Author = "Dummy Author",
            ReleaseDate = DateTime.Now,
            GenreId = 1
        };
        var correctBookSerialized = JsonConvert.SerializeObject(correctBook);

        var invalidBook = new CreateBookCommand
        {
            Title = "Dummy Title",
            Author = "Dummy Author",
            ReleaseDate = DateTime.Now,
            GenreId = 123
        };
        var invalidBookSerialized = JsonConvert.SerializeObject(invalidBook);

        var responseCorrect = await client.PostAsync("api/BookStore",
            new StringContent(correctBookSerialized, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.NoContent, responseCorrect.StatusCode);

        var responseInvalid = await client.PostAsync("api/BookStore",
            new StringContent(invalidBookSerialized, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.InternalServerError, responseInvalid.StatusCode);
    }

    [Fact]
    public async Task Put_UpdateBookInDatabase()
    {
        var client = _factory.CreateClient();

        var correctBook = new BookDto
        {
            Title = "Dammy Title",
            Author = "Dammy Author",
            ReleaseDate = DateTime.Now,
            GenreId = 1
        };
        var correctBookSerialized = JsonConvert.SerializeObject(correctBook);

        var invalidBook = new BookDto
        {
            Title = "Dammy Title",
            Author = "Dammy Author",
            ReleaseDate = DateTime.Now,
            GenreId = 123
        };
        var invalidBookSerialized = JsonConvert.SerializeObject(invalidBook);

        var responseCorrect = await client.PutAsync("api/BookStore/1",
            new StringContent(correctBookSerialized, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.NoContent, responseCorrect.StatusCode);

        var responseInvalid = await client.PutAsync("api/BookStore/1",
            new StringContent(invalidBookSerialized, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.InternalServerError, responseInvalid.StatusCode);

        var responseInvalidSecond = await client.PutAsync("api/BookStore/11111",
            new StringContent(invalidBookSerialized, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.NotFound, responseInvalidSecond.StatusCode);
    }

    [Fact]
    public async Task Delete_DeleteBookFromDatabase()
    {
        var client = _factory.CreateClient();

        var responseCorrect = await client.DeleteAsync("api/BookStore/1");
        Assert.Equal(HttpStatusCode.NoContent, responseCorrect.StatusCode);

        var responseInvalid = await client.DeleteAsync("api/BookStore/11111");
        Assert.Equal(HttpStatusCode.NotFound, responseInvalid.StatusCode);
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

        var book = dbContext.Books.FirstOrDefault(b => b.Id == 2);
        Assert.Equal("Diummy Book", book.Title);

        var responseInvalid = await client.PatchAsync("api/BookStore/111111",
            new StringContent(commandSerialized, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.NotFound, responseInvalid.StatusCode);
    }
}