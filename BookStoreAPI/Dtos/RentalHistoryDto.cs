using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreAPI.Models;

public class RentalHistoryDto
{
    [NotMapped] public int Id { get; set; }

    public int BookId { get; set; }

    public int ClientId { get; set; }

    [Required] public DateTime RentDate { get; set; }

    public DateTime? ReturnDate { get; set; }
}