namespace MAR.Domain.Models;

using System.ComponentModel.DataAnnotations;

public class Comment : BaseModel
{
    [Required]
    public required string Content { get; set; }

    [Required]
    public required int UserId { get; init; }

    [Required]
    public required int AlbumId { get; init; }
}