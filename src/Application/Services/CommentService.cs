namespace MAR.Application.Services;

using MAR.Domain.Interfaces;
using MAR.Domain.Models;

public class CommentService : BaseService<Comment>, ICommentService
{
    public CommentService(ICommentRepository repository, ILoggerAdapter<Comment> logger) 
        : base(repository, logger)
        {

        }
}