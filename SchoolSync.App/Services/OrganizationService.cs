using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class OrganizationService(IOrganizationRepo organizationRepo)
    : GenericService<Organization>(organizationRepo), IOrganizationService
{

    public override async Task ValidateCreateAsync(Organization entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Name))
            throw new ArgumentException("Organization name is required.");
        if (string.IsNullOrWhiteSpace(entity.Email) || !entity.Email.Contains('@'))
            throw new ArgumentException("Valid organization email is required.");

        // Uniqueness: Name already exists
        if (await _repo.ExistsAsync(x => x.Name == entity.Name))
            throw new ArgumentException("An organization with this name already exists.", nameof(entity.Name));

        // Uniqueness: Email already exists
        if (await _repo.ExistsAsync(x => x.Email == entity.Email))
            throw new ArgumentException("An organization with this email already exists.", nameof(entity.Email));

        // Uniqueness: Phone number already exists (if provided)
        if (!string.IsNullOrWhiteSpace(entity.PhoneNumber))
        {
            if (await _repo.ExistsAsync(x => x.PhoneNumber == entity.PhoneNumber))
                throw new ArgumentException("An organization with this phone number already exists.", nameof(entity.PhoneNumber));
        }
    }

    public override async Task ValidateUpdateAsync(Organization entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Name))
            throw new ArgumentException("Organization name is required.");
        if (string.IsNullOrWhiteSpace(entity.Email) || !entity.Email.Contains('@'))
            throw new ArgumentException("Valid organization email is required.");

        // Uniqueness: Name already exists (exclude self)
        if (await _repo.ExistsAsync(x => x.Name == entity.Name && x.Id != entity.Id))
            throw new ArgumentException("An organization with this name already exists.", nameof(entity.Name));

        // Uniqueness: Email already exists (exclude self)
        if (await _repo.ExistsAsync(x => x.Email == entity.Email && x.Id != entity.Id))
            throw new ArgumentException("An organization with this email already exists.", nameof(entity.Email));

        // Uniqueness: Phone number already exists (if provided, exclude self)
        if (!string.IsNullOrWhiteSpace(entity.PhoneNumber))
        {
            if (await _repo.ExistsAsync(x => x.PhoneNumber == entity.PhoneNumber && x.Id != entity.Id))
                throw new ArgumentException("An organization with this phone number already exists.", nameof(entity.PhoneNumber));
        }
    }
}
