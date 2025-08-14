using SchoolSync.Domain.Entities;

namespace SchoolSync.Domain.IServices;

public interface ISubjectService : IGenericService<Subject>
{
    Task<IEnumerable<Subject>> GetBySchoolAsync(int schoolId);
    Task<IEnumerable<Subject>> GetByTeacherAsync(int teacherId);
}
