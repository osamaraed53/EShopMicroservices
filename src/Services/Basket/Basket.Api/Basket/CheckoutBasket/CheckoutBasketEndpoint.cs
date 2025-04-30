using Basket.API.Dtos;

namespace Basket.Api.Basket.CheckoutBasket;
public record CheckoutBasketResponse(bool IsSuccess);
public record CheckoutBasketRequest(BasketCheckoutDto BasketCheckoutDto);
public class CheckoutBasketCommandValidator
    : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto can't be null");
        RuleFor(x => x.BasketCheckoutDto.UserName).NotEmpty().WithMessage("UserName is required");
    }
}
public class CheckoutBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/checkout",async (CheckoutBasketRequest request, ISender sender) => {

            var command = request.Adapt<CheckoutBasketCommand>();

            var result = await sender.Send(command);

            var response = request.Adapt<CheckoutBasketResponse>();

            return Results.Ok(result);

        }).WithName("Checkout Basket")
        .Produces<CheckoutBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Checkout Basket")
        .WithDescription("Checkout Basket");
    }
}
