using Basket.Api.Exception;
using Marten;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Basket.Api.Data;

public class BasketRepository(IDocumentSession session) : IBasketRepository
{
    public async Task<bool> DeleteBaseket(string userName, CancellationToken cancellationToken = default)
    {
        session.Delete<ShoppingCart>(userName);
        await session.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<ShoppingCart> GetBaseket(string userName, CancellationToken cancellationToken = default)
    {
        var basket = await session.LoadAsync<ShoppingCart>(userName, cancellationToken);
        return basket is null ? throw new BasketNotFoundException(userName) : basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        session.Store(basket);
        await session.SaveChangesAsync(cancellationToken);
        return basket;
    }
}
