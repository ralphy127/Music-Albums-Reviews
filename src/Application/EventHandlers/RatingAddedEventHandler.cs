namespace MAR.Application.EventHandlers;

using MAR.Domain.Events;
using MAR.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

public class RatingAddedEventHandler : IDomainEventHandler<RatingAddedEvent>
{
    private readonly IAlbumRepository _albumRepository;

    public RatingAddedEventHandler(IAlbumRepository albumRepository)
    {
        _albumRepository = albumRepository;
    }

    public async Task Handle(RatingAddedEvent domainEvent, CancellationToken cancellationToken) 
    {
        var album = await _albumRepository.GetByIdAsync(domainEvent.Rating.AlbumId);

        if (album is not null)
        {
            album.UpdateAverageRating();
            await _albumRepository.UpdateAsync(album);
        }
    }
}