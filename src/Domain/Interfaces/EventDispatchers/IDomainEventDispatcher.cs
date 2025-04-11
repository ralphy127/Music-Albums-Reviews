namespace MAR.Domain.Interfaces;

using System.Threading;
using System.Threading.Tasks;

public interface IDomainEventDispatcher 
{
    Task Dispatch<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default) where TEvent : IDomainEvent;
    
    void Subscribe<TEvent>(IDomainEventHandler<TEvent> handler) where TEvent : IDomainEvent;

    void AddHandler<TEvent>(Action<TEvent> handler) where TEvent : IDomainEvent;

    void RemoveHandler<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent;
}