using AutoMapper;
using BookStoreAPI.Exceptions;
using BookStoreAPI.Interfaces;
using BookStoreAPI.Models;
using MediatR;

namespace BookStoreAPI.Queries.Handlers;

public class GetClientQueryHandler : IRequestHandler<GetClientQuery, ClientDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetClientQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ClientDto> Handle(GetClientQuery request, CancellationToken cancellationToken)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(request.ClientId);
        ApiExceptionHandler.ThrowIf(client is null, 404, $"Client with ID {request.ClientId} doesn't exist.");

        var clientDto = _mapper.Map<ClientDto>(client);
        return clientDto;
    }
}