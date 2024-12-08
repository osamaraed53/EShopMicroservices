
namespace Basket.Api.Basket.StoreBasket;


public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor( c => c.Cart ).NotEmpty().WithMessage("Cart Can't be null");
        RuleFor(c => c.Cart.UserName).NotEmpty().WithMessage("UserName is Required");
    }
}

internal class StoreBasketCommandHandler(IBasketRepository repository) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {

        var response = await repository.StoreBasket(command.Cart, cancellationToken);


        return new StoreBasketResult(response.UserName);
    }
}
