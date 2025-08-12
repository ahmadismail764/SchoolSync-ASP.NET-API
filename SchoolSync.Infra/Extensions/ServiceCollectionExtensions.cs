using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolSync.Infra.Persistence;
using SchoolSync.Infra.Seeders;
using Microsoft.EntityFrameworkCore;
namespace SchoolSync.Infra.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Add DbContext
        services.AddDbContext<DBContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("DevDB"))
        );

        // Register repos later

        // Register seeders
        services.AddScoped<IDBSeeder, DBSeeder>();
        return services;
    }

    public static async Task<IServiceProvider> SeedDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DBContext>();
        var seeder = scope.ServiceProvider.GetRequiredService<IDBSeeder>();
        await context.Database.EnsureCreatedAsync();
        await seeder.SeedAsync();
        return serviceProvider;
    }
}
