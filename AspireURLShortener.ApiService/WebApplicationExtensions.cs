using AspireURLShortener.ApiService.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace AspireURLShortener.ApiService;

public static class WebApplicationExtensions
{
    public static async Task ConfigureDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Ensure the database is created
        await EnsureDatabaseAsync(dbContext);

        // Run migrations
        await RunMigrationsAsync(dbContext);
    }
    
    private static async Task EnsureDatabaseAsync(ApplicationDbContext dbContext)
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();
        
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            if (!await dbCreator.ExistsAsync())
            {
                await dbCreator.CreateAsync();
            }
        });
    }
    
    private static async Task RunMigrationsAsync(ApplicationDbContext dbContext)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await dbContext.Database.MigrateAsync();
        });
    }
}