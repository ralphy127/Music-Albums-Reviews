namespace MAR.Application.Services;

using MAR.Domain.Events;
using MAR.Domain.Interfaces;
using MAR.Domain.Models;

public class RatingService : BaseService<Rating>, IRatingService
{
    private readonly IDomainEventDispatcher _eventDispatcher;

    private readonly IAlbumRepository _albumRepository;

    public RatingService(IRatingRepository repository, IAlbumRepository albumRepository, ILoggerAdapter<Rating> logger, IDomainEventDispatcher eventDispatcher) 
        : base(repository, logger)
    {
        _albumRepository = albumRepository;
        _eventDispatcher = eventDispatcher;
    }

    public override async Task AddAsync(Rating rating)
    {
        await base.AddAsync(rating);
        await _eventDispatcher.Dispatch(new RatingAddedEvent(rating));

        var album = await _albumRepository.GetByIdAsync(rating.AlbumId);
        if ((album is not null) && (album.StoredAverageRating >= 8.0))
        {
            await _eventDispatcher.Dispatch(new RatingTresholdReachedEvent(album));
        }
    }
}