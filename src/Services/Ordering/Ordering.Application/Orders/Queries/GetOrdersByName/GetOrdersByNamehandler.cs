
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Extentions;

namespace Ordering.Application.Orders.Queries.GetOrdersByName;

internal class GetOrdersByNamehandler(IApplicationDbContext context) : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
{
    public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
    {
        var orderName = OrderName.Of(query.Name);

        var orders = await context.Orders
             .AsNoTracking()
             .Where(e => e.OrderName == orderName)
             .OrderBy(e => e.OrderName.Value)
             .ToListAsync(cancellationToken);


        return new GetOrdersByNameResult(orders.ToOrderDtoList());

    }
}
