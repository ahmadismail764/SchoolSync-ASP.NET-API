using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Infra.Persistence;
namespace SchoolSync.Infra.Repositories;

public class SchoolYearRepo(DBContext context) : GenericRepo<SchoolYear>(context), ISchoolYearRepo
{
    public async Task<IEnumerable<SchoolYear>> GetBySchoolAsync(int schoolId)
        => await GetRangeWhereAsync(s => s.SchoolId == schoolId);
}
