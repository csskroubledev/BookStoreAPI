using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediatR;

namespace BookStoreAPI.Commands;

public class UpdateBookCommand : IRequest<Unit>
{
    [NotMapped] public int BookId { get; set; }

    public int? ClientId { get; set; }

    [Required] [StringLength(50)] public string Title { get; set; } = string.Empty;

    [Required] [StringLength(50)] public string Author { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime ReleaseDate { get; set; }

    [Required] public decimal Price { get; set; }

    [Required] public int GenreId { get; set; } = default!;
}