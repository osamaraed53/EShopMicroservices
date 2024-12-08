namespace Basket.Api.Data;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBaseket(string userName, CancellationToken cancellationToken = default);
    Task<ShoppingCart> StoreBasket(ShoppingCart cart, CancellationToken cancellationToken = default);
    Task<bool> DeleteBaseket(string userName, CancellationToken cancellationToken = default);
}