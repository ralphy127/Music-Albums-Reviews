namespace MAR.Domain.Interfaces;

using MAR.Domain.Models;

public interface IRatingRepository : IBaseRepository<Rating>
{
    Task<IEnumerable<Rating>> GetAllByUserIdAsync(int userId);
    Task DeleteAllByUserIdAsync(int userId);
}