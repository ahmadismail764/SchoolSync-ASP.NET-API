using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class SchoolService(ISchoolRepo schoolRepo, IOrganizationRepo organizationRepo)
    : GenericService<School>(schoolRepo), ISchoolService
{
    private readonly IOrganizationRepo _organizationRepo = organizationRepo;

    public async Task<School?> GetByOrganizationAsync(int orgId) => await schoolRepo.GetByOrganizationAsync(orgId);
    public async Task<School?> GetByNameAsync(string name) => await schoolRepo.GetByNameAsync(name);

    public override async Task ValidateCreateAsync(School entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Name))
            throw new ArgumentException("School name is required.");
        if (string.IsNullOrWhiteSpace(entity.Email) || !entity.Email.Contains('@'))
            throw new ArgumentException("Valid school email is required.");
        if (entity.OrganizationId <= 0)
            throw new ArgumentException("OrganizationId must be set.");
        // Cross-entity: Organization must exist and be active
        var org = await _organizationRepo.GetAsync(entity.OrganizationId);
        if (org == null || !org.IsActive)
            throw new ArgumentException("Organization must exist and be active.");

        // Uniqueness: Name must be unique per Organization
        var existing = await _repo.GetRangeWhereAsync(x => x.Name == entity.Name && x.OrganizationId == entity.OrganizationId);
        if (existing.Any())
            throw new ArgumentException("A school with this name already exists in the organization.", nameof(entity.Name));
    }

    public override async Task ValidateUpdateAsync(School entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Name))
            throw new ArgumentException("School name is required.");
        if (string.IsNullOrWhiteSpace(entity.Email) || !entity.Email.Contains('@'))
            throw new ArgumentException("Valid school email is required.");
        if (entity.OrganizationId <= 0)
            throw new ArgumentException("OrganizationId must be set.");
        // Cross-entity: Organization must exist and be active
        var org = await _organizationRepo.GetAsync(entity.OrganizationId);
        if (org == null || !org.IsActive)
            throw new ArgumentException("Organization must exist and be active.");
    }
}
