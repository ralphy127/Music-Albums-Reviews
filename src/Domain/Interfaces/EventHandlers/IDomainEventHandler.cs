namespace MAR.Domain.Interfaces;

using System.Threading;
using System.Threading.Tasks;

public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
{
    Task Handle(TEvent domainEvent, CancellationToken cancellationToken = default);
}