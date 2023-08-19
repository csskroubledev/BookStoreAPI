using System.Net;
using System.Text;
using AutoMapper;
using BookStoreAPI.Commands;
using BookStoreAPI.Functions.Commands.Client.Create;
using BookStoreAPI.Functions.Commands.Client.Patch;
using BookStoreAPI.Models;
using BookStoreAPI.Tests.Utils;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace BookStoreAPI.Tests.IntegrationTests.Clients;

public class ClientsTest : IClassFixture<BookStoreWebApplicationFactory<Program>>
{
    private readonly BookStoreWebApplicationFactory<Program> _factory;
    private readonly IMapper _mapper;
    private readonly IServiceProvider _serviceProvider;

    public ClientsTest(BookStoreWebApplicationFactory<Program> factory)
    {
        _factory = factory;

        _serviceProvider = _factory.Services;
        _mapper = _serviceProvider.GetRequiredService<IMapper>();

        Data.SeedData(_serviceProvider);
    }

    [Fact]
    public async Task Get_ReturnsListOfAllClientsFromDatabase()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("api/Client");
        response.EnsureSuccessStatusCode();

        var contentString = await response.Content.ReadAsStringAsync();
        var clients = JsonConvert.DeserializeObject<IEnumerable<ClientDto>>(contentString);

        Assert.NotNull(clients);
        Assert.NotEmpty(clients);
    }

    [Fact]
    public async Task Get_ReturnsClientFromDatabase()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BookStoreDatabaseContext>();

        var client = _factory.CreateClient();

        var firstClient = dbContext.Clients.First();

        var response = await client.GetAsync($"api/Client/{firstClient.Id}");
        response.EnsureSuccessStatusCode();

        var contentString = await response.Content.ReadAsStringAsync();
        var DbClient = JsonConvert.DeserializeObject<ClientDto>(contentString);
        var firstClientDto = _mapper.Map<ClientDto>(firstClient);

        Assert.NotNull(DbClient);
        Assert.Equivalent(firstClientDto, DbClient);

        var responseInvalid = await client.GetAsync("api/Client/11111");
        Assert.Equal(HttpStatusCode.NotFound, responseInvalid.StatusCode);
    }

    [Fact]
    public async Task Post_AddsClientToDatabase()
    {
        var client = _factory.CreateClient();

        var clientCommand = new CreateClientCommand
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = DateTime.Now
        };
        var clientCommandSerialized = JsonConvert.SerializeObject(clientCommand);

        var clientInvalidCommand = new CreateClientCommand
        {
            FirstName =
                "Johnaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaabbbaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            LastName = "Doe",
            DateOfBirth = DateTime.Now
        };
        var clientInvalidCommandSerialized = JsonConvert.SerializeObject(clientInvalidCommand);

        var response = await client.PostAsync("api/Client",
            new StringContent(clientCommandSerialized, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var responseInvalid = await client.PostAsync("api/Client",
            new StringContent(clientInvalidCommandSerialized, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.BadRequest, responseInvalid.StatusCode);
    }

    [Fact]
    public async Task Put_UpdateClientInDatabase()
    {
        var client = _factory.CreateClient();

        var clientCommand = new UpdateClientCommand
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = DateTime.Now
        };
        var clientCommandSerialized = JsonConvert.SerializeObject(clientCommand);

        var clientInvalidCommand = new UpdateClientCommand
        {
            FirstName =
                "Johnaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaabbbaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            LastName = "Doe",
            DateOfBirth = DateTime.Now
        };
        var clientInvalidCommandSerialized = JsonConvert.SerializeObject(clientInvalidCommand);

        var response = await client.PutAsync("api/Client/1",
            new StringContent(clientCommandSerialized, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var responseInvalid = await client.PutAsync("api/Client/1",
            new StringContent(clientInvalidCommandSerialized, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.BadRequest, responseInvalid.StatusCode);

        var responseInvalidSecond = await client.PutAsync("api/Client/11111111",
            new StringContent(clientCommandSerialized, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.NotFound, responseInvalidSecond.StatusCode);
    }

    [Fact]
    public async Task Delete_DeleteClientFromDatabase()
    {
        var client = _factory.CreateClient();

        var responseCorrect = await client.DeleteAsync("api/Client/1");
        Assert.Equal(HttpStatusCode.NoContent, responseCorrect.StatusCode);

        var responseInvalid = await client.DeleteAsync("api/Client/11111");
        Assert.Equal(HttpStatusCode.NotFound, responseInvalid.StatusCode);
    }

    [Fact]
    public async Task Patch_UpdateClientInDatabase()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BookStoreDatabaseContext>();

        var client = _factory.CreateClient();

        var command = new PatchClientCommand
        {
            FirstName = "Adam"
        };
        var commandSerialized = JsonConvert.SerializeObject(command);

        var response = await client.PatchAsync("api/Client/2",
            new StringContent(commandSerialized, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var dbClient = dbContext.Clients.FirstOrDefault(c => c.Id == 2);
        Assert.Equal("Adam", dbClient.FirstName);

        var responseInvalid = await client.PatchAsync("api/Client/111111",
            new StringContent(commandSerialized, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.NotFound, responseInvalid.StatusCode);
    }
}