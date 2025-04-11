namespace MAR.Infrastructure.EventDispatchers;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MAR.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class DomainEventDispatcher : IDomainEventDispatcher 
{
    private readonly IServiceProvider _serviceProvider;

    private readonly ILoggerAdapter<DomainEventDispatcher> _logger;

    private readonly ConcurrentDictionary<Type, List<Type>> _handlers;

    private readonly ConcurrentDictionary<Type, List<Delegate>> _actionHandlers;

    public DomainEventDispatcher(IServiceProvider serviceProvider, ILoggerAdapter<DomainEventDispatcher> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _handlers = new();
        _actionHandlers = new();
    }

    public async Task Dispatch<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default) where TEvent : IDomainEvent
    {
        var eventType = typeof(TEvent);

        if (!_handlers.TryGetValue(eventType, out var handlerTypes) && handlerTypes is not null) 
        {
            foreach (var handlerType in handlerTypes)
            {
                if(_serviceProvider.GetService(handlerType) is IDomainEventHandler<TEvent> handler)
                {
                    try 
                    {
                        await handler.Handle(domainEvent, cancellationToken);
                        _logger.LogInformation($"Handled event {eventType.Name} by handler {handlerType.Name}.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error handling event {eventType.Name} by handler {handlerType.Name}.");
                        throw;
                    }
                }
            }
        }

        
        if (_actionHandlers.TryGetValue(eventType, out var actionDelegates))
        {
            foreach (var action in actionDelegates.Cast<Action<TEvent>>())
            {
                try
                {
                    action.Invoke(domainEvent);
                    _logger.LogInformation($"Handled event {eventType.Name} by action handler");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error invoking action handler for event {eventType.Name}");
                    throw;
                }
            }
        }
    }

    public void Subscribe<TEvent>(IDomainEventHandler<TEvent> handler) where TEvent : IDomainEvent
    {
        var eventType = typeof(TEvent);
        var handlerType = handler.GetType();

         _logger.LogInformation($"Subscribing handler {handlerType.Name} for event {eventType.Name}");

        _handlers.AddOrUpdate(
            eventType,
            new List<Type> { handlerType },
            (_, existingList) =>
            {
                if (!existingList.Contains(handlerType)) 
                {
                    var newList = new List<Type>(existingList) { handlerType };
                    return newList;
                }

                return existingList;
            });
    }

    public void AddHandler<TEvent>(Action<TEvent> handler) where TEvent : IDomainEvent
    {
        var eventType = typeof(TEvent);

         _logger.LogInformation($"Adding dynamic handler for event {eventType.Name}");

        _actionHandlers.AddOrUpdate(
            eventType,
            new List<Delegate> { handler },
            (_, existingList) =>
            {
                if (!existingList.Contains(handler))
                {
                    var newList = new List<Delegate>(existingList) { handler };
                    return newList;
                }

                return existingList;
            });
    }

    public void RemoveHandler<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent
    {
        var eventType = typeof(TEvent);

        _logger.LogInformation($"Removing dynamic handler for event {eventType.Name}");

        if (_actionHandlers.TryGetValue(eventType, out var existingList))
        {
            existingList.RemoveAll(handler => handler is Action<TEvent> action && Equals(action.Target, domainEvent));

            if (existingList.Count == 0)
            {
                _actionHandlers.TryRemove(eventType, out _);
            }
        }
    }    
}