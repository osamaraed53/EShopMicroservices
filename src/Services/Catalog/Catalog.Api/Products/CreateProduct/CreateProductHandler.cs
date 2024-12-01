
namespace Catalog.Api.Products.CreateProduct;



public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);


public class CreateProductCommandValidator  :
    AbstractValidator<CreateProductCommand>
{

    public CreateProductCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage("Name is Required");
        RuleFor(c => c.Category).NotEmpty().WithMessage("Name is Required");
        RuleFor(c => c.ImageFile).NotEmpty().WithMessage("ImageFile is Required");
        RuleFor(c => c.Price).GreaterThan(0).WithMessage("Price must be grater than 0");
    }

}


public class CreateProductCommandHandler(IDocumentSession session ,ILogger<CreateProductCommand> logger)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("CreateProductCommand.Handle called with {@command}", command);


        // nested of this I am use MediateR Pipline Bhaviours 
        //var result = await validator.ValidateAsync(command, cancellationToken);
        //var errors = result.Errors.Select(x => x.ErrorMessage).ToList();
        //if (errors.Count != 0)
        //{
        //    throw new ValidationException(errors.FirstOrDefault());
        //}

        //create Product Entity From command 
        Product product =  new()
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price,
        };

        //save to data base 
        session.Store(product);
        // TODO : I need understand cancellationToken in more deeply 
        await session.SaveChangesAsync(cancellationToken);


        //return Result
        return new CreateProductResult(product.Id);


    }
}
