using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediatR;

namespace BookStoreAPI.Functions.Commands.Client.Patch;

public class PatchClientCommand : IRequest<Unit>
{
    [NotMapped]
    public int ClientId { get; set; }

    [StringLength(50)]
    public string? FirstName { get; set; }
    
    [StringLength(50)]
    public string? LastName { get; set; }
    
    [DataType(DataType.DateTime)]
    public DateTime? DateOfBirth { get; set; } = null;
}