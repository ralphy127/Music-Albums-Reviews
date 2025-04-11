namespace MAR.Infrastructure.Repositories;

using System.Security.Cryptography.X509Certificates;
using MAR.Domain.Interfaces;
using MAR.Domain.Models;
using MAR.Infrastructure.Persistance;

public class AlbumRepository : BaseRepository<Album>, IAlbumRepository
{
    public AlbumRepository(ApplicationDbContext dbContext) 
        : base(dbContext) 
    {

    }

    public async Task UpdateAverageRating(int albumId)
    {
        var album = await GetByIdAsync(albumId);

        if (album is not null)
        {
            album.UpdateAverageRating();
            await _dbContext.SaveChangesAsync();
        }
    }
}