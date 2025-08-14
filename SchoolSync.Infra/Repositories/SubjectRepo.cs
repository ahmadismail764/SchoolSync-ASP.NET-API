using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace SchoolSync.Infra.Repositories;

public class SubjectRepo: GenericRepo<Subject>, ISubjectRepo
{
    public SubjectRepo(DbContext context) : base(context) { }
    public async Task<IEnumerable<Subject>> GetBySchoolAsync(int schoolId) 
    {
        return await GetRangeWhereAsync(s => s.SchoolId == schoolId);
    }
    public async Task<IEnumerable<Subject>> GetByTeacherAsync(int teacherId)
    {
        return await GetRangeWhereAsync(s => s.TeacherId == teacherId);
    }
}
