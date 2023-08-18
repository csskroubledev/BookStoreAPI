using MediatR;

namespace BookStoreAPI.Functions.Commands.Client.Delete;

public class DeleteClientCommand : IRequest<Unit>
{
    public int ClientId { get; set; }
}