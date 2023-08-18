using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreAPI.Models;

public class ClientDto
{
    [NotMapped]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime DateOfBirth { get; set; }
    
    [NotMapped]
    public ICollection<BookDto> RentedBooks { get; set; } = default!;
}