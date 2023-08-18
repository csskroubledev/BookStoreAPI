using BookStoreAPI.Exceptions;
using BookStoreAPI.Interfaces;
using MediatR;

namespace BookStoreAPI.Functions.Commands.Book.Delete;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Unit>
{
    private IUnitOfWork _unitOfWork;

    public DeleteBookCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var bookToDelete = await _unitOfWork.Books.GetByIdAsync(request.BookId);
        ApiExceptionHandler.ThrowIf(bookToDelete is null, 404, $"Book with ID {request.BookId} doesn't exist.");

        _unitOfWork.Books.Delete(bookToDelete);

        await _unitOfWork.SaveAsync();
        
        return Unit.Value;
    }
}