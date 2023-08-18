using BookStoreAPI.Commands;
using BookStoreAPI.Functions.Commands.Client.Create;
using BookStoreAPI.Functions.Commands.Client.Delete;
using BookStoreAPI.Functions.Commands.Client.Patch;
using BookStoreAPI.Models;
using BookStoreAPI.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<ClientDto>> Get()
    {
        var request = new GetAllClientsQuery();

        var result = await _mediator.Send(request);

        return result;
    }

    [HttpGet("{id}", Name = "GetClientById")]
    public async Task<ClientDto> Get(int id)
    {
        var request = new GetClientQuery
        {
            ClientId = id
        };

        var result = await _mediator.Send(request);

        return result;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CreateClientCommand createClientCommand)
    {
        await _mediator.Send(createClientCommand);

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] UpdateClientCommand updateClientCommand)
    {
        updateClientCommand.ClientId = id;
        await _mediator.Send(updateClientCommand);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteClientCommand
        {
            ClientId = id
        };
        await _mediator.Send(command);

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> Patch(int id, [FromBody] PatchClientCommand patchClientCommand)
    {
        patchClientCommand.ClientId = id;
        await _mediator.Send(patchClientCommand);

        return NoContent();
    }
}