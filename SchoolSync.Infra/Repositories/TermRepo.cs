using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace SchoolSync.Infra.Repositories;

public class TermRepo: GenericRepo<Term>, ITermRepo
{
    public TermRepo(DbContext context) : base(context)
    {
    }
    public async Task<IEnumerable<Term>> GetBySchoolYearAsync(int schoolYearId)
    {
        return await GetRangeWhereAsync(term => term.SchoolYearId == schoolYearId);
    }
}
{
}
