using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.Entities;
using SchoolSync.Infra.Persistence;
namespace SchoolSync.Infra.Repositories;

public class TermRepo(DBContext context) : GenericRepo<Term>(context), ITermRepo
{
    public async Task<IEnumerable<Term>> GetBySchoolYearAsync(int schoolYearId)
    {
        return await GetRangeWhereAsync(term => term.SchoolYearId == schoolYearId);
    }
}