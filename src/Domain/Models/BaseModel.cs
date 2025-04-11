namespace MAR.Domain.Models;

using System.ComponentModel.DataAnnotations;

public class BaseModel 
{
    [Key]
    public int Id { get; set; }

    public DateTime CreationTime { get; } = DateTime.UtcNow;
}