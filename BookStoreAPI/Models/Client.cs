namespace BookStoreAPI.Models;

public class Client
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    public DateTime DateOfBirth { get; set; }

    public ICollection<Book> RentedBooks { get; set; } = default!;
}