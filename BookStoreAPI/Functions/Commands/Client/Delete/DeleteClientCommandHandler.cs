using BookStoreAPI.Exceptions;
using BookStoreAPI.Interfaces;
using MediatR;

namespace BookStoreAPI.Functions.Commands.Client.Delete;

public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteClientCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        var clientToDelete = await _unitOfWork.Clients.GetByIdAsync(request.ClientId);
        ApiExceptionHandler.ThrowIf(clientToDelete is null, 404, $"Client with ID {request.ClientId} doesn't exist.");

        _unitOfWork.Clients.Delete(clientToDelete);

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}