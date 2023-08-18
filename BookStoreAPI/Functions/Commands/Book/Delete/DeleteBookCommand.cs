using MediatR;

namespace BookStoreAPI.Functions.Commands.Book.Delete;

public class DeleteBookCommand : IRequest<Unit>
{
    public int BookId { get; set; }
}