using AutoMapper;
using BookStoreAPI.Exceptions;
using BookStoreAPI.Interfaces;
using BookStoreAPI.Models;
using MediatR;

namespace BookStoreAPI.Commands.Handlers;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Unit>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var bookToEdit = await _unitOfWork.Books.GetByIdAsync(request.BookId);
        ApiExceptionHandler.ThrowIf(bookToEdit is null, 404, $"Book with ID {request.BookId} doesn't exist.");

        _unitOfWork.Books.Update(bookToEdit);

        if (request.ClientId is not null && bookToEdit.ClientId != request.ClientId)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(request.ClientId.Value);
            ApiExceptionHandler.ThrowIf(client is null, 404, "Client with specified ID doesn't exist.");

            if (bookToEdit.ClientId is not null)
            {
                var currentRental =
                    bookToEdit.RentalHistory.SingleOrDefault(r =>
                        r.ClientId == bookToEdit.ClientId && r.ReturnDate == null);
                if (currentRental is not null) currentRental.ReturnDate = DateTime.Now;
            }

            var rentalHistoryEntry = new RentalHistory
            {
                BookId = bookToEdit.Id,
                ClientId = request.ClientId.Value,
                RentDate = DateTime.Now
            };

            bookToEdit.ClientId = request.ClientId;
            bookToEdit.RentalHistory.Add(rentalHistoryEntry);
        }

        if (request.ClientId is null && bookToEdit.ClientId is not null)
        {
            var currentRental =
                bookToEdit.RentalHistory.SingleOrDefault(r =>
                    r.ClientId == bookToEdit.ClientId && r.ReturnDate == null);
            if (currentRental is not null) currentRental.ReturnDate = DateTime.Now;

            bookToEdit.ClientId = null;
        }

        _mapper.Map(request, bookToEdit);

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}