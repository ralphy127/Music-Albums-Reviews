namespace MAR.Domain.Interfaces;

public interface IDomainEvent 
{
    DateTime OccuredOn { get; }

    Guid Id { get; }
}