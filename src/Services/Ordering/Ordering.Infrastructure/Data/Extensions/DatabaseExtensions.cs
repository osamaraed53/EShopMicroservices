using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure.Data.Extensions;

public static class DatabaseExtensions
{

    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {

        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.MigrateAsync().GetAwaiter().GetResult();

        await SeedDataAsync(context, context.Customers, InitialData.Customers);
        await SeedDataAsync(context, context.Products, InitialData.Products);
        await SeedDataAsync(context, context.Orders, InitialData.OrdersWithItems);
    }


    private static async Task SeedDataAsync<T>(ApplicationDbContext context, DbSet<T> dbSet, IEnumerable<T> initialData) where T : class
    {
        if (!await dbSet.AnyAsync())
        {
            await dbSet.AddRangeAsync(initialData);
            await context.SaveChangesAsync();
        }
    }
}
