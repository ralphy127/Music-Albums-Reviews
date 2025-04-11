namespace MAR.Presentation.Controllers;

using Microsoft.AspNetCore.Mvc;
using MAR.Domain.Interfaces;
using MAR.Domain.Models;

[Route("api/users")]
[ApiController]
public class UserController : BaseController<User>
{
    public UserController(IUserService service, ILoggerAdapter<User> logger)
        : base(service, logger)
    {
        
    }
}