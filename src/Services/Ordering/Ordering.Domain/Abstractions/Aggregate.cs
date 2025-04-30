
namespace Ordering.Domain.Abstractions;

public abstract class Aggregate<TId> : Entity<TId>, IAggregate<TId>
{
    private readonly List<IDomainEvent> _domainEvent = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvent.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvent.Add(domainEvent);
    }

    public IDomainEvent[] ClearDomainEvent()
    {
        IDomainEvent[] dequeuedEvents = [.. _domainEvent];

        _domainEvent.Clear();

        return dequeuedEvents;
    }
}
