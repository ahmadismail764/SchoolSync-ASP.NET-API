using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.Entities;
using SchoolSync.Infra.Persistence;
namespace SchoolSync.Infra.Repositories;

public class SubjectRepo(DBContext context) : GenericRepo<Subject>(context), ISubjectRepo
{
    public async Task<IEnumerable<Subject>> GetBySchoolAsync(int schoolId) 
    {
        return await GetRangeWhereAsync(s => s.SchoolId == schoolId);
    }
    public async Task<IEnumerable<Subject>> GetByTeacherAsync(int teacherId)
    {
        return await GetRangeWhereAsync(s => s.TeacherId == teacherId);
    }
}
