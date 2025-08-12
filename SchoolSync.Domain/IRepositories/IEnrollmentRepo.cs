using SchoolSync.Domain.Entities;
namespace SchoolSync.Domain.IRepositories;

public interface IEnrollmentRepo : IGenericRepo<Enrollment>
{
    Task<IEnumerable<Enrollment>> GetByStudentAsync(int studentId);
    Task<IEnumerable<Enrollment>> GetBySubjectAsync(int subjectId);
    Task<IEnumerable<Enrollment>> GetByTermAsync(int termId);
    Task<Enrollment?> GetByCompositeKeyAsync(int studentId, int subjectId, int termId);
}
