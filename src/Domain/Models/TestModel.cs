namespace MAR.Domain.Models;

using System.ComponentModel.DataAnnotations;

public class TestModel : BaseModel
{
    [Required]
    public required string Name { get; set; }
}