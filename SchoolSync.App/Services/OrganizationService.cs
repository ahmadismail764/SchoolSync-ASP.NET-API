using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class OrganizationService(IOrganizationRepo organizationRepo)
    : GenericService<Organization>(organizationRepo), IOrganizationService
{
    private readonly IOrganizationRepo _organizationRepo = organizationRepo;
}
