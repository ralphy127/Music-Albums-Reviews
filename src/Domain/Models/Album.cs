namespace MAR.Domain.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Album : BaseModel
{
    [Required]
    public required string Title { get; set; }

    [Required]
    public required string Artist { get; set; }

    [Required]
    public required int Year { get; set; }

    [Required]
    public required string Description { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    [NotMapped]
    public float? AverageRating => CalculateAverageRating();

    [Column("AverageRating")]
    public float? StoredAverageRating { get; private set; }

    public float? CalculateAverageRating()
    {
        if (Ratings is null || !Ratings.Any())
        {
            return null;
        }

        return (float)Math.Round(Ratings.Average(r => r.Score), 1);
    }

    public void UpdateAverageRating()
    {
        StoredAverageRating = CalculateAverageRating();
    }
}