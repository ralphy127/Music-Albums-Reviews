namespace MAR.Domain.Models;

using System.ComponentModel.DataAnnotations;

public class User : BaseModel
{
    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Email { get; set; }

    [Required]
    public required string HashedPassword { get; set; }
}