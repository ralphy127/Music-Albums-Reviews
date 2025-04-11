namespace MAR.Application.Services;

using MAR.Domain.Interfaces;
using MAR.Domain.Models;

public class AlbumService : BaseService<Album>, IAlbumService
{
    public AlbumService(IAlbumRepository repository, ILoggerAdapter<Album> logger) 
        : base(repository, logger) 
    {
            
    }
}