using Basket.Api.Data;

namespace Basket.Api.Basket.GetBasket;
public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
public record GetBasketResult(ShoppingCart Cart);
internal class GetBasketHandler(IBasketRepository repository) : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
       var basket  =  await repository.GetBaseket(query.UserName, cancellationToken);

        return new GetBasketResult(basket);
    }
}
