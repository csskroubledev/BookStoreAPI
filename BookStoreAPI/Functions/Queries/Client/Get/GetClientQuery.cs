using BookStoreAPI.Models;
using MediatR;

namespace BookStoreAPI.Queries;

public class GetClientQuery : IRequest<ClientDto>
{
    public int ClientId { get; set; }
}