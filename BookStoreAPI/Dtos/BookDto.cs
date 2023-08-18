using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreAPI.Models;

public class BookDto
{
    [NotMapped] public int BookId { get; set; }

    public int? ClientId { get; set; }
    public ICollection<RentalHistoryDto> RentalHistory { get; set; } = new List<RentalHistoryDto>();

    [Required] [StringLength(50)] public string Title { get; set; } = string.Empty;

    [Required] [StringLength(50)] public string Author { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime ReleaseDate { get; set; }


    [Required] public decimal Price { get; set; }

    [Required] public int GenreId { get; set; } = default!;
}