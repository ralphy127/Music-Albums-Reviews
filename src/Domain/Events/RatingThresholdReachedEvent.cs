namespace MAR.Domain.Events;

using MAR.Domain.Interfaces;
using MAR.Domain.Models;

public class RatingTresholdReachedEvent : IDomainEvent
{
    public Guid Id { get; }

    public DateTime OccuredOn { get; }

    public Album Album { get; }

    public RatingTresholdReachedEvent(Album album)
    {
        Id = Guid.NewGuid();
        OccuredOn = DateTime.Now;
        Album = album;
    }
}