namespace MAR.Infrastructure.Repositories;

using MAR.Domain.Interfaces;
using MAR.Domain.Models;
using MAR.Infrastructure.Persistance;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) 
        : base(dbContext) 
    {
        
    }
}