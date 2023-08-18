using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Models;

public class RentalHistory
{
    public int Id { get; set; }

    public int BookId { get; set; }
    public virtual Book Book { get; set; }

    public int ClientId { get; set; }
    public virtual Client Client { get; set; }

    [Required] public DateTime RentDate { get; set; }

    public DateTime? ReturnDate { get; set; }
}