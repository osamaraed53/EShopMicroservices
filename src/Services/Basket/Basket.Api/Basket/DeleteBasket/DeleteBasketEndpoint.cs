﻿namespace Basket.Api.Basket.DeleteBasket;


//public record DeleteBasketRequest(string UserName); 
public record DeleteBasketResponse(bool IsSuccess);

public class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
        {

            var request = await sender.Send(new DeleteBasketCommand(userName));

            var response = request.Adapt<DeleteBasketResponse>();

            return Results.Ok(response);

        }).WithName("Delete Basket")
        .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Delete Basket")
        .WithDescription("Delete Basket");
        
    }
}
