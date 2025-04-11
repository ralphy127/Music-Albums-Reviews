namespace MAR.Infrastructure.Repositories;

using MAR.Domain.Interfaces;
using MAR.Domain.Models;
using MAR.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

public class CommentRepository : BaseRepository<Comment>, ICommentRepository
{
    public CommentRepository(ApplicationDbContext dbContext) 
        : base(dbContext) 
    { 
        
    }

    public async Task DeleteAllByUserIdAsync(int userId)
    {
        var comments = await _dbSet.Where(c => c.UserId == userId).ToListAsync();
        _dbSet.RemoveRange(comments);
        await _dbContext.SaveChangesAsync();
    }
}