using SchoolSync.Domain.Entities;

namespace SchoolSync.Domain.IServices;

public interface IEnrollmentService : IGenericService<Enrollment>
{
    Task<IEnumerable<Enrollment>> GetByStudentAsync(int studentId);
    Task<IEnumerable<Enrollment>> GetBySubjectAsync(int subjectId);

    Task<IEnumerable<Enrollment>> GetByTermAsync(int termId);
    Task<Enrollment?> GetByCompositeKeyAsync(int studentId, int subjectId, int termId);

}
