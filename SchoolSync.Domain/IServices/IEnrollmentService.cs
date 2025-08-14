using SchoolSync.Domain.Entities;

namespace SchoolSync.Domain.IServices;

public interface IEnrollmentService : IGenericService<Enrollment>
{
    Task<IEnumerable<Enrollment>> GetByStudentAsync(int studentId);
    Task<IEnumerable<Enrollment>> GetBySubjectAsync(int subjectId);
}
