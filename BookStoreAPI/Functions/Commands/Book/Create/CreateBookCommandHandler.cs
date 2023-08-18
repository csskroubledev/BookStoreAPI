using BookStoreAPI.Interfaces;
using MediatR;

namespace BookStoreAPI.Functions.Commands.Book.Create;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var bookToAdd = new Models.Book
        {
            Title = request.Title,
            Author = request.Author,
            ReleaseDate = request.ReleaseDate,
            Price = request.Price,
            GenreId = request.GenreId
        };

        await _unitOfWork.Books.AddAsync(bookToAdd);

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}