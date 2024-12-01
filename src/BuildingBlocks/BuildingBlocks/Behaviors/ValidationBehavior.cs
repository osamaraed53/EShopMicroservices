using MediatR;
using BuildingBlocks.CQRS;
using FluentValidation;


namespace BuildingBlocks.Behaviors;


// The ValidationBehavior<TRequest, TResponse> class you provided is a MediatR pipeline behavior that handles validation for incoming commands.
public class ValidationBehavior<TRequest, TResponse>
    //accepts an IEnumerable<IValidator<TRequest>> validators, which allows for multiple validators to be injected. This approach provides flexibility for complex validation scenarios where a command might need multiple rules.
    (IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    //Handle Method: This is where the main logic occurs. It takes TRequest request, a delegate next to proceed with the next handler, and a CancellationToken.
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        //Validation Context: A ValidationContext<TRequest> object is created for the incoming request. This context is necessary for the validators to perform their checks.
        var context = new ValidationContext<TRequest>(request);

        //Asynchronous Validation: The line await Task.WhenAll.. performs validation using all the injected validators. Each validator's ValidateAsync method runs concurrently.
        var validationResults = 
            await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));


        //Handling Validation Results: The results of all validations are collected in validationResults. After that, it filters for any validation failures and accumulates them into a list called failures.
        var failures = 
            validationResults
            .Where(r => r.Errors.Count != 0)
            .SelectMany(r => r.Errors)
            .ToList();


        //Exception Handling: If any validation errors are present, a ValidationException is thrown with the collected failures, preventing the request from being processed further.
        if (failures.Count != 0)
        {
            throw new ValidationException(failures);
        }

        // Continuing the Pipeline: If there are no validation issues, it calls await next();, which proceeds to the actual request handling logic.
        return await next();

    }

    //This pattern decouples validation from command handlers, centralizes validation logic, and promotes code reusability, making it easier to maintain and modify validation rules across your application.


}
