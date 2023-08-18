using BookStoreAPI.Models;
using MediatR;

namespace BookStoreAPI.Queries;

public class GetBookQuery : IRequest<BookDto>
{
    public int BookId { get; set; }
}