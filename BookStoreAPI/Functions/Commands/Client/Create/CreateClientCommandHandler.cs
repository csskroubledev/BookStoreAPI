using AutoMapper;
using BookStoreAPI.Interfaces;
using MediatR;

namespace BookStoreAPI.Functions.Commands.Client.Create;

public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Unit>
{
    private IUnitOfWork _unitOfWork;

    public CreateClientCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var clientToAdd = new Models.Client
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth
        };

        await _unitOfWork.Clients.AddAsync(clientToAdd);

        await _unitOfWork.SaveAsync();
        
        return Unit.Value;
    }
}