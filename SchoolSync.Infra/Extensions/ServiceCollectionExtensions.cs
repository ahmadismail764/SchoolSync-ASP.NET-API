using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Infra.Persistence;
using SchoolSync.Infra.Repositories;
using SchoolSync.Infra.Seeders;
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

        // Register repositories
        services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));

        services.AddScoped<IEnrollmentRepo, EnrollmentRepo>();
        services.AddScoped<IOrganizationRepo, OrganizationRepo>();
        services.AddScoped<ISchoolRepo, SchoolRepo>();
        services.AddScoped<ISchoolYearRepo, SchoolYearRepo>();
        services.AddScoped<ITermRepo, TermRepo>();
        services.AddScoped<ISubjectRepo, SubjectRepo>();
        services.AddScoped<IUserRepo, UserRepo>();
        services.AddScoped<ILessonRepo, LessonRepo>();
        services.AddScoped<IMaterialRepo, MaterialRepo>();
        services.AddScoped<IEmailVerificationRepo, EmailVerificationRepo>();

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

        Console.WriteLine("Seeding database with initial data...");
        await seeder.SeedAsync();
        Console.WriteLine("Database seeding completed.");
        return serviceProvider;
    }
}
