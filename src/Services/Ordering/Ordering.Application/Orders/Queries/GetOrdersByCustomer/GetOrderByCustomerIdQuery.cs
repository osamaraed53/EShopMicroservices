namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer;

public record GetOrderByCustomerIdQuery(Guid Id) : IQuery<GetOrderByCustomerIdResult>;

public record GetOrderByCustomerIdResult(IEnumerable<OrderDto> Orders);
