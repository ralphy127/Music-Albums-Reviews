namespace MAR.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using MAR.Domain.Interfaces;
using MAR.Domain.Models;
using MAR.Infrastructure.Persistance;

public class BaseRepository<TType> : IBaseRepository<TType> where TType : BaseModel
{
    protected readonly ApplicationDbContext _dbContext;

    protected readonly DbSet<TType> _dbSet;

    public BaseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<TType>();
    }

    public async Task<IEnumerable<TType>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<TType?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<TType?> GetByFieldAsync<TField>(string fieldName, TField value)
    {
        return await _dbSet.FirstOrDefaultAsync(e =>
                (EF.Property<TField>(e, fieldName) ?? default)!.Equals(value)
            );
    }

    public async Task AddAsync(TType entity)
    {
        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(TType entity)
    {
        _dbSet.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(TType entity)
    {
        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}