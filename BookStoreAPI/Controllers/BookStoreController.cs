using AutoMapper;
using BookStoreAPI.Commands;
using Microsoft.AspNetCore.JsonPatch;
using BookStoreAPI.Exceptions;
using BookStoreAPI.Functions.Commands.Book.Create;
using BookStoreAPI.Functions.Commands.Book.Delete;
using BookStoreAPI.Functions.Commands.Book.Patch;
using BookStoreAPI.Interfaces;
using BookStoreAPI.Models;
using BookStoreAPI.Queries;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookStoreController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookStoreController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> Get()
    {
        var request = new GetAllBooksQuery();
        var result = await _mediator.Send(request);

        return Ok(result);
    }

    [HttpGet("{id}", Name = "GetBookById")]
    public async Task<ActionResult<BookDto>> Get(int id)
    {
        var request = new GetBookQuery
        {
            BookId = id
        };
        
        var result = await _mediator.Send(request);
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CreateBookCommand createBookCommand)
    {
        await _mediator.Send(createBookCommand);

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] UpdateBookCommand updateBookCommand)
    {
        updateBookCommand.BookId = id;
        await _mediator.Send(updateBookCommand);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteBookCommand
        {
            BookId = id
        };
        await _mediator.Send(command);

        return NoContent();
    }
    
    [HttpPatch("{id}")]
    public async Task<ActionResult> Patch(int id, [FromBody] PatchBookCommand patchBookCommand)
    {
        patchBookCommand.BookId = id;
        await _mediator.Send(patchBookCommand);

        return NoContent();
    }
}