using AutoMapper;
using BookStoreAPI.Interfaces;
using BookStoreAPI.Models;
using MediatR;

namespace BookStoreAPI.Queries.Handlers;

public class GetAllClientsQueryHandler : IRequestHandler<GetAllClientsQuery, IEnumerable<ClientDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetAllClientsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ClientDto>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
    {
        var clients = await _unitOfWork.Clients.GetAllAsync();

        var clientsDto = _mapper.Map<IEnumerable<ClientDto>>(clients);
        return clientsDto;
    }
}