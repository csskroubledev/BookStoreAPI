using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Models;

public class Book
{
    public int Id { get; set; }
    public int? ClientId { get; set; }
    public virtual Client? Client { get; set; }

    public ICollection<RentalHistory> RentalHistory { get; set; } = new List<RentalHistory>();

    [Required] [StringLength(50)] public string Title { get; set; } = string.Empty;

    [Required] [StringLength(100)] public string Author { get; set; } = string.Empty;

    [Required] [DataType(DataType.Time)] public DateTime ReleaseDate { get; set; }

    public decimal Price { get; set; }

    public int GenreId { get; set; }
    public Genre? Genre { get; set; } = default!;
}