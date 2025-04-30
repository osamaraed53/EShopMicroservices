using BuildingBlocks.Exceptions.Handler;
using Discount.Grpc;
using HealthChecks.UI.Client;
using System.Reflection;
using BuildingBlocks.Messaging.MassTarnsit;

var builder = WebApplication.CreateBuilder(args);


// add services to the container 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IBasketRepository ,BasketRepository>();
builder.Services.Decorate<IBasketRepository ,CachedBasketRepository>();

string databaseConictionString = builder.Configuration.GetConnectionString("Database")!;
string redisConictionString = builder.Configuration.GetConnectionString("Redis")!;


// Application Services 
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

//Data Services
builder.Services.AddMarten(options =>
{
    options.Connection(databaseConictionString);
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName);

}).UseLightweightSessions();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConictionString;
});

//Grpc Services
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new(builder.Configuration["GrpcSettings:DiscountUrl"]!);
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    //for development purpose
    HttpClientHandler handler = new()
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
    return handler;
});


//Cross-Cutting Services
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(databaseConictionString)
    .AddRedis(redisConictionString);


//Async Communication Services
// because we are in publisher we not provide Assembly
builder.Services.AddMessageBroker(builder.Configuration);

var app = builder.Build();
//Config the HTTP request pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// the empty option indicates that we are relying on custom configured handler
app.UseExceptionHandler(option => { });
app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapCarter();


app.Run();
