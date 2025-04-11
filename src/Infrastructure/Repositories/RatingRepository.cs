namespace MAR.Infrastructure.Repositories;

using MAR.Domain.Interfaces;
using MAR.Domain.Models;
using MAR.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

public class RatingRepository : BaseRepository<Rating>, IRatingRepository
{
    public RatingRepository(ApplicationDbContext dbContext) 
        : base(dbContext) 
    {

    }

    public async Task<IEnumerable<Rating>> GetAllByUserIdAsync(int userId)
    {
        return await _dbSet.Where(r => r.UserId == userId).ToListAsync();
    }

    public async Task DeleteAllByUserIdAsync(int userId)
    {
        var ratings = await GetAllByUserIdAsync(userId);
        _dbSet.RemoveRange(ratings);
        await _dbContext.SaveChangesAsync();
    }
}