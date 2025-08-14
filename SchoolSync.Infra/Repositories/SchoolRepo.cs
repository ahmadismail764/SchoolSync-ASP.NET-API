using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace SchoolSync.Infra.Repositories;

public class SchoolRepo : GenericRepo<School>, ISchoolRepo
{
    private readonly DbSet<School> _schools;

    public SchoolRepo(DbContext context) : base(context)
    {
        _schools = context.Set<School>();
    }

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
