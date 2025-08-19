using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;
namespace SchoolSync.App.Services;

public class SchoolService(ISchoolRepo schoolRepo) : GenericService<School>(schoolRepo), ISchoolService
{
    private readonly ISchoolRepo _schoolRepo = schoolRepo;
    public async Task<School?> GetByOrganizationAsync(int orgId) => await _schoolRepo.GetByOrganizationAsync(orgId);
    public async Task<School?> GetByNameAsync(string name) => await _schoolRepo.GetByNameAsync(name);

}
