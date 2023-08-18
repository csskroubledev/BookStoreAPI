using System.ComponentModel.DataAnnotations;
using MediatR;

namespace BookStoreAPI.Functions.Commands.Client.Create;

public class CreateClientCommand : IRequest<Unit>
{
    [Required] [StringLength(50)] public string FirstName { get; set; } = string.Empty;

    [Required] [StringLength(50)] public string LastName { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime DateOfBirth { get; set; }
}