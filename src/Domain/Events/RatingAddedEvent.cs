namespace MAR.Domain.Events;

using MAR.Domain.Interfaces;
using MAR.Domain.Models;

public class RatingAddedEvent : IDomainEvent {
    public Guid Id { get; }
    
    public DateTime OccuredOn { get; }

    public Rating Rating { get; }

    public RatingAddedEvent(Rating rating) 
    {
        Id = Guid.NewGuid();
        OccuredOn = DateTime.Now;
        Rating = rating;
    }
}