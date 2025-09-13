using Microsoft.Extensions.DependencyInjection;
using SchoolSync.Domain.IServices;
using SchoolSync.App.Services;
using System.Reflection;
namespace SchoolSync.App.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddScoped<ISchoolService, SchoolService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEnrollmentService, EnrollmentService>();
        services.AddScoped<ISubjectService, SubjectService>();
        services.AddScoped<ITermService, TermService>();
        services.AddScoped<ISchoolYearService, SchoolYearService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IMaterialService, MaterialService>();
        services.AddScoped<ILessonService, LessonService>();
        services.AddScoped<IEmailVerificationService, EmailVerificationService>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        return services;
    }
}
