using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class OrganizationService(IOrganizationRepo organizationRepo)
    : GenericService<Organization>(organizationRepo), IOrganizationService
{
    // private readonly ISchoolRepo _schoolRepo = schoolRepo;

    public override async Task ValidateCreateAsync(Organization entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Name))
            throw new ArgumentException("Organization name is required.");
        if (string.IsNullOrWhiteSpace(entity.Email) || !entity.Email.Contains('@'))
            throw new ArgumentException("Valid organization email is required.");

        // Uniqueness: Name must be unique
        var existing = await _repo.GetRangeWhereAsync(x => x.Name == entity.Name);
        if (existing.Any())
            throw new ArgumentException("An organization with this name already exists.", nameof(entity.Name));
    }

    public override Task ValidateUpdateAsync(Organization entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Name))
            throw new ArgumentException("Organization name is required.");
        if (string.IsNullOrWhiteSpace(entity.Email) || !entity.Email.Contains('@'))
            throw new ArgumentException("Valid organization email is required.");
        return Task.CompletedTask;
    }
}
