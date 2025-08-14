using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.Entities;
using SchoolSync.Infra.Persistence;
namespace SchoolSync.Infra.Repositories;

public class SchoolRepo(DBContext context) : GenericRepo<School>(context), ISchoolRepo
{
    public async Task<School?> GetByOrganizationAsync(int orgId)
    {
        var schools = await GetRangeWhereAsync(s => s.OrganizationId == orgId);
        return schools.FirstOrDefault();
    }

    public async Task<School?> GetByNameAsync(string name)
    {
        var schools = await GetRangeWhereAsync(s => s.Name == name);
        return schools.FirstOrDefault();
    }
}
