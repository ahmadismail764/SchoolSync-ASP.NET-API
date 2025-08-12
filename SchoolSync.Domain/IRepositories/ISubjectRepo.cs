using SchoolSync.Domain.Entities;
namespace SchoolSync.Domain.IRepositories;

public interface ISubjectRepo : IGenericRepo<Subject>
{
    Task<IEnumerable<Subject>> GetBySchoolAsync(int schoolId);
    Task<IEnumerable<Subject>> GetByTeacherAsync(int teacherId);
}