using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class OrganizationService(IOrganizationRepo organizationRepo)
    : GenericService<Organization>(organizationRepo), IOrganizationService
{
    // private readonly ISchoolRepo _schoolRepo = schoolRepo;

    public override Task ValidateAsync(Organization entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Name))
            throw new ArgumentException("Organization name is required.");
        if (string.IsNullOrWhiteSpace(entity.Email) || !entity.Email.Contains('@'))
            throw new ArgumentException("Valid organization email is required.");
        // if (!entity.IsActive)
        //     throw new InvalidOperationException("Cannot deactivate organization with active schools. Deactivate schools first.");

        return Task.CompletedTask;
    }
}
