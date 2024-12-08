
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Api.Data;

public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
{
    public async Task<bool> DeleteBaseket(string userName, CancellationToken cancellationToken = default)
    {

        await repository.DeleteBaseket(userName, cancellationToken);

        await cache.RemoveAsync(userName, cancellationToken);

        return true;
    }

    public async Task<ShoppingCart> GetBaseket(string userName, CancellationToken cancellationToken = default)
    {
        var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
        if (!string.IsNullOrEmpty(cachedBasket))
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)! ;
        

        var basket = await repository.GetBaseket(userName, cancellationToken);
        await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket),cancellationToken);
        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await repository.StoreBasket(basket, cancellationToken);
        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket),cancellationToken);
        
        return basket;
    }
}
