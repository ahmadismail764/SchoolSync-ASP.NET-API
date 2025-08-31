using SchoolSync.Domain.Entities;
namespace SchoolSync.Domain.IServices;

public interface ILessonService : IGenericService<Lesson>
{
    Task<IEnumerable<Lesson>> GetBySubjectAsync(int SubjectId);
}
