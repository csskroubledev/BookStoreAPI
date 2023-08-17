namespace BookStoreAPI.Models;

public class BookDto
{
    public int BookId { get; set; }
    public int? ClientId { get; set; }
    
    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public DateTime ReleaseDate { get; set; }
    public DateTime? RentDate { get; set; }

    public decimal Price { get; set; }

    public int GenreId { get; set; } = default!;
}