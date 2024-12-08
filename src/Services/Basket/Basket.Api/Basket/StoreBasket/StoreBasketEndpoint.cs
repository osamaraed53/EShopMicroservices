namespace Basket.Api.Basket.StoreBasket;

public record StoreBasketRequest(ShoppingCart Cart);
public record StoreBasketResponse(string UserName);
public class StoreBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (StoreBasketRequest request,
            ISender sender) =>
        {
            var commnd = request.Adapt<StoreBasketCommand>();

            var result = await sender.Send(commnd);

            var response = result.Adapt<StoreBasketResponse>();

            return Results.Created($"/basket/{response.UserName}",response);

        }).WithName("StoreBasketEndpoint")
        .Produces<StoreBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("StoreBasketEndpoint")
        .WithDescription("StoreBasketEndpoint"); 
    }
}
