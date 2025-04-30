using BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Extentions;

namespace Ordering.Application.Orders.Queries.GetOrders;

internal class GetOrdershandler(IApplicationDbContext context) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var pageIndex  = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        var totalCount = await context.Orders.LongCountAsync(cancellationToken);

        var orders = await context.Orders
            .Include(e=>e.OrderItems)
            .OrderBy(e=>e.OrderName.Value)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new GetOrdersResult(
            new PaginatedResult<OrderDto>(
                PageIndex : pageIndex,
                PageSize : pageSize,
                Count: totalCount,
                Data : orders.ToOrderDtoList()
                )           
            );

        throw new NotImplementedException();
    }
}
