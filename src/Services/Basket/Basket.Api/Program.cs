

using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);
// add services to the container 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCarter();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository , CachedBasketRepository>();

string databaseConictionString = builder.Configuration.GetConnectionString("Database")!;
string redisConictionString = builder.Configuration.GetConnectionString("Redis")!;

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConictionString;
});


var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));

});

builder.Services.AddMarten(options =>
{
    options.Connection(databaseConictionString);
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName );
}).UseLightweightSessions();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(databaseConictionString)
    .AddRedis(redisConictionString);

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
