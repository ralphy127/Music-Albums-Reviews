namespace MAR.Domain.Events;

using MAR.Domain.Models;
using MAR.Domain.Interfaces;

public class UserDeletedEvent : IDomainEvent
{
    public Guid Id { get; }

    public DateTime OccuredOn { get; }

    public User User { get; }

    public UserDeletedEvent(User user)
    {
        Id = Guid.NewGuid();
        OccuredOn = DateTime.Now;
        User = user;
    }
}