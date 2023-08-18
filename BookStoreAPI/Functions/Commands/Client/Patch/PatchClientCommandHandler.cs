using BookStoreAPI.Exceptions;
using BookStoreAPI.Interfaces;
using MediatR;

namespace BookStoreAPI.Functions.Commands.Client.Patch;

public class PatchClientCommandHandler : IRequestHandler<PatchClientCommand, Unit>
{
    private IUnitOfWork _unitOfWork;

    public PatchClientCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(PatchClientCommand request, CancellationToken cancellationToken)
    {
        var clientToEdit = await _unitOfWork.Clients.GetByIdAsync(request.ClientId);
        ApiExceptionHandler.ThrowIf(clientToEdit is null, 404, "Client with specified ID doesn't exist.");
        
        _unitOfWork.Clients.Update(clientToEdit);
        
        clientToEdit.FirstName = request.FirstName ?? clientToEdit.FirstName;
        clientToEdit.LastName = request.LastName ?? clientToEdit.LastName;
        clientToEdit.DateOfBirth = request.DateOfBirth ?? clientToEdit.DateOfBirth;

        await _unitOfWork.SaveAsync();
        
        return Unit.Value;
    }
}