
using Catalog.Api.Products.GetProductById;

namespace Catalog.Api.Products.DeleteProduct;

//public record DeleteProductRequest(Guid Id);

public record DeleteProductResponse(bool IsSuccess);
public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
       app.MapDelete("/product/{id}" , async (Guid id , ISender sender) =>
       {
           var result = await sender.Send(new DeleteProductCommand(id));
           
           var response = result.Adapt<DeleteProductResponse>();

           return Results.Ok(response);

       }).WithName("Delete Product")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Delete Product")
        .WithDescription("Update Product");
    }
}
