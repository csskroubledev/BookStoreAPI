using AutoMapper;
using BookStoreAPI.Exceptions;
using BookStoreAPI.Interfaces;
using MediatR;

namespace BookStoreAPI.Commands.Handlers;

public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, Unit>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var clientToEdit = await _unitOfWork.Clients.GetByIdAsync(request.ClientId);
        ApiExceptionHandler.ThrowIf(clientToEdit is null, 404, "Client with specified ID doesn't exist.");

        _unitOfWork.Clients.Update(clientToEdit);

        _mapper.Map(request, clientToEdit);

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}