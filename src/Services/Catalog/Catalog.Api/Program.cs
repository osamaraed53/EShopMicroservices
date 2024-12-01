
using BuildingBlocks.Exceptions.Handler;

var builder = WebApplication.CreateBuilder(args);

// add services to the container 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    //Registering Services from Assembly: The method config.RegisterServicesFromAssemblies(typeof(Program).Assembly); tells MediatR to scan the current assembly (where the Program class is located) for all implementations of IRequest<TResponse>, IRequestHandler<TRequest, TResponse>, and notification handlers. This registration ensures that any commands, queries, and their respective handlers defined in the same assembly are automatically registered.
    config.RegisterServicesFromAssemblies(assembly);


    //Adding Pipeline Behavior: The line config.AddOpenBehavior(typeof(ValidationBehavior<,>)); registers the ValidationBehavior<TRequest, TResponse> as a pipeline behavior for MediatR. Pipeline behaviors allow you to introduce additional processing logic (like validation, logging, etc.) before and after the main request handler is executed. By using AddOpenBehavior, it enables the specified behavior for any request and response types handled by MediatR, making it flexible and reusable.
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

// is used to register all the validators defined in the specified assembly, typically the assembly where your main application code resides.
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();


//Config the HTTP request pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();

// the empty option indicates that we are relying on custom configured handler
app.UseExceptionHandler(option => { });

//Exception handler lambda 
//app.UseExceptionHandler(exceptionHandlerApp =>
//{

//    exceptionHandlerApp.Run(async context =>
//    {
//        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

//        if (exception is null)
//        {
//            return;
//        }
//        var problemDetails = new ProblemDetails()
//        {
//            Title = exception.Message,
//            Status = StatusCodes.Status500InternalServerError,
//            Detail = exception.StackTrace,
//        };

//        var logger  = context.RequestServices.GetRequiredService<ILogger<Program>>();
//        logger.LogError(exception, message: exception.Message);

//        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
//        context.Response.ContentType = "application/program+json";

//        await context.Response.WriteAsJsonAsync(problemDetails);


//    });


//});

app.Run();
