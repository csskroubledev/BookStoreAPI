using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediatR;

namespace BookStoreAPI.Functions.Commands.Book.Patch;

public class PatchBookCommand : IRequest<Unit>
{
    [NotMapped] public int BookId { get; set; }

    public int? ClientId { get; set; }

    [StringLength(50)] public string? Title { get; set; }

    [StringLength(50)] public string? Author { get; set; }

    [DataType(DataType.DateTime)] public DateTime? ReleaseDate { get; set; }

    public decimal? Price { get; set; }

    public int? GenreId { get; set; }
}