using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;
namespace SchoolSync.App.Services;

public class SchoolService(ISchoolRepo schoolRepo) : ISchoolService
{
    private readonly ISchoolRepo _schoolRepo = schoolRepo;

    public async Task<School?> GetByIdAsync(int id) => await _schoolRepo.GetAsync(id);
    public async Task<IEnumerable<School>> GetAllAsync() => await _schoolRepo.GetAllAsync();
    public async Task<School?> GetByOrganizationAsync(int orgId) => await _schoolRepo.GetByOrganizationAsync(orgId);
    public async Task<School?> GetByNameAsync(string name) => await _schoolRepo.GetByNameAsync(name);
    public async Task CreateAsync(School school) => await _schoolRepo.CreateAsync(school);
    public async Task UpdateAsync(School school) => await _schoolRepo.UpdateAsync(school);
    public async Task DeleteAsync(int id)
    {
        var school = await _schoolRepo.GetAsync(id);
        if (school != null)
            await _schoolRepo.DeleteAsync(school);
    }
}
