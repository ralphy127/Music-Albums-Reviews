namespace MAR.Presentation.Controllers;

using Microsoft.AspNetCore.Mvc;
using MAR.Domain.Interfaces;
using MAR.Domain.Models;

[Route("api/rating")]
[ApiController]
public class RatingController : BaseController<Rating>
{
    public RatingController(IRatingService service, ILoggerAdapter<Rating> logger)
        : base(service, logger)
    {
        
    }
}