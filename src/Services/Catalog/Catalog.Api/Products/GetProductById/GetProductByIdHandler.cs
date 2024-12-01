

namespace Catalog.Api.Products.GetProductById;

public record class GetProductByIdQuery(Guid Id) 
    : IQuery<GetProductByIdResult>;

public record class GetProductByIdResult(Product Product);



internal class GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductByIdQuery.Handle called with {@Query}", query);

        var products = await session.LoadAsync<Product>(query.Id , cancellationToken);

        return products is null ? throw new ProductNotFoundException(query.Id) : new GetProductByIdResult(products);
    }
}


