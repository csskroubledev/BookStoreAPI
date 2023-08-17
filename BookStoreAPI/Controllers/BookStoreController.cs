using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using BookStoreAPI.Exceptions;
using BookStoreAPI.Interfaces;
using BookStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookStoreController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IBookService _bookService;

    public BookStoreController(IMapper mapper, IBookService bookService)
    {
        _mapper = mapper;
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<IEnumerable<BookDto>> Get()
    {
        return _mapper.Map<IEnumerable<BookDto>>(await _bookService.GetAllBooksAsync());
    }

    [HttpGet("{id}", Name = "GetBookById")]
    public async Task<BookDto> Get(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        ApiExceptionHandler.ThrowIf(book is null, 404, "Book with specified ID doesn't exist.");

        return _mapper.Map<BookDto>(book);
    }

    [HttpPost]
    public async Task Post([FromBody] BookDto data)
    {
        var book = _mapper.Map<Book>(data);
        await _bookService.AddBookAsync(book);
    }

    [HttpPut("{id}")]
    public async Task Put(int id, [FromBody] BookDto data)
    {
        var book = _mapper.Map<Book>(data);
        await _bookService.UpdateBookAsync(id, book);
    }

    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        await _bookService.DeleteBookAsync(id);
    }
    
    [HttpPatch("{id}")]
    public async Task Patch(int id, [FromBody] BookPatchDto data)
    {
        await _bookService.PatchBookAsync(id, data);
    }
}