using BookStoreAPI.Models;
using MediatR;

namespace BookStoreAPI.Queries;

public class GetAllBooksQuery : IRequest<IEnumerable<BookDto>>
{
}