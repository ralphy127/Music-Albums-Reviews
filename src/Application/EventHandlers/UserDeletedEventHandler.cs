namespace MAR.Application.EventHandlers;

using MAR.Domain.Events;
using MAR.Domain.Interfaces;
using MAR.Domain.Models;
using MAR.Infrastructure.Repositories;
using System.Threading;
using System.Threading.Tasks;

public class UserDeletedEventHandler : IDomainEventHandler<UserDeletedEvent>
{
    private readonly IAlbumRepository _albumRepository;

    private readonly ICommentRepository _commentRepository;

    private readonly IRatingRepository _ratingRepository;

    public UserDeletedEventHandler(IAlbumRepository albumRepository, ICommentRepository commentRepository, IRatingRepository ratingRepository)
    {
        _albumRepository = albumRepository;
        _commentRepository = commentRepository;
        _ratingRepository = ratingRepository;
    }

    public async Task Handle(UserDeletedEvent domainEvent, CancellationToken cancellationToken)
    {
        var userId = domainEvent.User.Id;

        var userRatings = await _ratingRepository.GetAllByUserIdAsync(userId);
        var albumIds = userRatings.Select(r => r.AlbumId).ToList();

        await _ratingRepository.DeleteAllByUserIdAsync(userId);

        foreach (var albumId in albumIds)
        {
            await _albumRepository.UpdateAverageRating(albumId);
        }

        await _commentRepository.DeleteAllByUserIdAsync(userId);
    }
}