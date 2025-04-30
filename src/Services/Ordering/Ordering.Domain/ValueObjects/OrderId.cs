namespace Ordering.Domain.ValueObjects;

public record OrderId
{
    public Guid Value { get; }

    private OrderId(Guid value) => Value = value;

    public static OrderId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value == Guid.Empty ? throw new DomainException("OrderId can't be empty.") : new OrderId(value);
    }


}
