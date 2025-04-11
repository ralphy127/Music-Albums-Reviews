namespace MAR.Presentation.Controllers;

using Microsoft.AspNetCore.Mvc;
using MAR.Domain.Interfaces;
using MAR.Domain.Models;

[Route("api/comment")]
[ApiController]
public class CommentController : BaseController<Comment>
{
    public CommentController(ICommentService service, ILoggerAdapter<Comment> logger)
        : base(service, logger)
    {
        
    }
}