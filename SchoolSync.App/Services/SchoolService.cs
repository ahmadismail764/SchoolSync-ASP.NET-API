using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;
namespace SchoolSync.App.Services;

public class SchoolService(ISchoolRepo schoolRepo) : GenericService<School>(schoolRepo), ISchoolService
{
    private readonly ISchoolRepo _schoolRepo = schoolRepo;
    public async Task<School?> GetByOrganizationAsync(int orgId) => await _schoolRepo.GetByOrganizationAsync(orgId);
    public async Task<School?> GetByNameAsync(string name) => await _schoolRepo.GetByNameAsync(name);

    public override Task ValidateAsync(School entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Name))
            throw new ArgumentException("School name is required.");
        if (string.IsNullOrWhiteSpace(entity.Email) || !entity.Email.Contains('@'))
            throw new ArgumentException("Valid school email is required.");
        if (entity.OrganizationId <= 0)
            throw new ArgumentException("OrganizationId must be set.");
        return Task.CompletedTask;
    }
}
