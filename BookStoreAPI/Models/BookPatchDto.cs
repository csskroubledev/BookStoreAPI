namespace BookStoreAPI.Models;

public class BookPatchDto
{
    public int? BookId { get; set; }
    public int? ClientId { get; set; }
    
    public string? Title { get; set; }

    public string? Author { get; set; }

    public DateTime? ReleaseDate { get; set; }
    public DateTime? RentDate { get; set; }

    public decimal? Price { get; set; }

    public int? GenreId { get; set; }
}