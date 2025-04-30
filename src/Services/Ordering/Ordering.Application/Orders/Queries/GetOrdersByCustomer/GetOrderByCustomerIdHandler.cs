
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Extentions;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer;

internal class GetOrderByCustomerIdHandler(IApplicationDbContext context) : IQueryHandler<GetOrderByCustomerIdQuery, GetOrderByCustomerIdResult>
{
    public async Task<GetOrderByCustomerIdResult> Handle(GetOrderByCustomerIdQuery query, CancellationToken cancellationToken)
    {
        var customerId = CustomerId.Of(query.Id);

        var orders = await context.Orders
            .AsNoTracking()
            .Where(e => e.CustomerId == customerId)
            .Include(e => e.OrderItems)
            .OrderBy(e => e.OrderName.Value)
            .ToListAsync(cancellationToken);
        

        return new GetOrderByCustomerIdResult(orders.ToOrderDtoList());

    }
}
