namespace MAR.Domain.Interfaces;

using MAR.Domain.Models;

public interface ICommentRepository : IBaseRepository<Comment>
{
    Task DeleteAllByUserIdAsync(int userId);
}