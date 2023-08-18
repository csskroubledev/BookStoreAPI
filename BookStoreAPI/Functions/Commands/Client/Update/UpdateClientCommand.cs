using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediatR;

namespace BookStoreAPI.Commands;

public class UpdateClientCommand : IRequest<Unit>
{
    [NotMapped] public int ClientId { get; set; }

    [Required] [StringLength(50)] public string FirstName { get; set; } = string.Empty;

    [Required] [StringLength(50)] public string LastName { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime DateOfBirth { get; set; }
}