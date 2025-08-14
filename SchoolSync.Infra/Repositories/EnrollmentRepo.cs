using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace SchoolSync.Infra.Repositories;

public class EnrollmentRepo(DbContext context) : GenericRepo<Enrollment>(context), IEnrollmentRepo
{
    public async Task<IEnumerable<Enrollment>> GetByStudentAsync(int studentId)
    {
        return await GetRangeWhereAsync(e => e.StudentId == studentId);
    }
    public async Task<IEnumerable<Enrollment>> GetBySubjectAsync(int subjectId)
    {
        return await GetRangeWhereAsync(e => e.SubjectId == subjectId);
        
    }
    public async Task<IEnumerable<Enrollment>> GetByTermAsync(int termId)
    {
        return await GetRangeWhereAsync(e => e.TermId == termId);
    }
    public async Task<Enrollment?> GetByCompositeKeyAsync(int studentId, int subjectId, int termId)
    {
        return await GetRangeWhereAsync(e => e.StudentId == studentId && e.SubjectId == subjectId && e.TermId == termId)
            .ContinueWith(task => task.Result.FirstOrDefault());  
    }
}
