namespace MAR.Domain.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Rating : BaseModel
{
    [Required]
    public required int Score { get; set; }

    [Required]
    public required int UserId { get; init; }

    [Required]
    public required int AlbumId { get; init; }

    [ForeignKey(nameof(AlbumId))]
    public virtual Album? Album { get; set; }
}