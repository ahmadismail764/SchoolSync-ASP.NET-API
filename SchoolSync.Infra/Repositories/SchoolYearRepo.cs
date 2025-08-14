using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
namespace SchoolSync.Infra.Repositories;

public class SchoolYearRepo : GenericRepo<SchoolYear>, ISchoolYearRepo
{
    public SchoolYearRepo(DbContext context) : base(context)
    {

    }
    public async Task<IEnumerable<SchoolYear>> GetBySchoolAsync(int schoolId)
    {
        return await GetRangeWhereAsync(s => s.SchoolId == schoolId);
    }
}
