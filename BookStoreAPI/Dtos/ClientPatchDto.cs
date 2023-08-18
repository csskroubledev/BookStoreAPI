using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreAPI.Models;

public class ClientPatchDto
{
    [NotMapped]
    public int Id { get; set; }

    [StringLength(50)]
    public string? FirstName { get; set; }
    
    [StringLength(50)]
    public string? LastName { get; set; }
    
    [DataType(DataType.DateTime)]
    public DateTime? DateOfBirth { get; set; } = null;
}