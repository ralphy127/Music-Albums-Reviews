namespace MAR.Presentation.Controllers;

using Microsoft.AspNetCore.Mvc;
using MAR.Domain.Interfaces;
using MAR.Domain.Models;

[Route("api/album")]
[ApiController]
public class AlbumController : BaseController<Album>
{
    public AlbumController(IAlbumService service, ILoggerAdapter<Album> logger)
        : base(service, logger)
    {
        
    }
}