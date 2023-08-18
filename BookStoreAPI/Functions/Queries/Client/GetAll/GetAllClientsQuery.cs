using BookStoreAPI.Models;
using MediatR;

namespace BookStoreAPI.Queries;

public class GetAllClientsQuery : IRequest<IEnumerable<ClientDto>>
{
}