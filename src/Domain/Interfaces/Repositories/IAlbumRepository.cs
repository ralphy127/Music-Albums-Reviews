namespace MAR.Domain.Interfaces;

using MAR.Domain.Models;

public interface IAlbumRepository : IBaseRepository<Album>
{
    Task UpdateAverageRating(int albumId);
}