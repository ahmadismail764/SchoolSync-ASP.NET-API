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

        // Uniqueness: Name already exists per Organization
        if (await _repo.ExistsAsync(x => x.Name == entity.Name && x.OrganizationId == entity.OrganizationId))
            throw new ArgumentException("A school with this name already exists in the organization.", nameof(entity.Name));

        // Uniqueness: Email already exists
        if (await _repo.ExistsAsync(x => x.Email == entity.Email))
            throw new ArgumentException("A school with this email already exists.", nameof(entity.Email));

        // Uniqueness: Phone number already exists (if provided)
        if (!string.IsNullOrWhiteSpace(entity.PhoneNumber))
        {
            if (await _repo.ExistsAsync(x => x.PhoneNumber == entity.PhoneNumber))
                throw new ArgumentException("A school with this phone number already exists.", nameof(entity.PhoneNumber));
        }
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

        // Uniqueness: Name already exists per Organization (exclude self)
        if (await _repo.ExistsAsync(x => x.Name == entity.Name && x.OrganizationId == entity.OrganizationId && x.Id != entity.Id))
            throw new ArgumentException("A school with this name already exists in the organization.", nameof(entity.Name));

        // Uniqueness: Email already exists (exclude self)
        if (await _repo.ExistsAsync(x => x.Email == entity.Email && x.Id != entity.Id))
            throw new ArgumentException("A school with this email already exists.", nameof(entity.Email));

        // Uniqueness: Phone number already exists (if provided, exclude self)
        if (!string.IsNullOrWhiteSpace(entity.PhoneNumber))
        {
            if (await _repo.ExistsAsync(x => x.PhoneNumber == entity.PhoneNumber && x.Id != entity.Id))
                throw new ArgumentException("A school with this phone number already exists.", nameof(entity.PhoneNumber));
        }
    }
}
