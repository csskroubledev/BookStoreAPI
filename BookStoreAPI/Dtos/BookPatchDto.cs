using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreAPI.Models;

public class BookPatchDto
{
    [NotMapped]
    public int? BookId { get; set; }
    public int? ClientId { get; set; }
    
    [StringLength(50)]
    public string? Title { get; set; }

    [StringLength(50)]
    public string? Author { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? ReleaseDate { get; set; }
    [DataType(DataType.DateTime)]

    public decimal? Price { get; set; }

    public int? GenreId { get; set; }
}