using SchoolSync.Domain.Entities;

namespace SchoolSync.Domain.IRepositories;

public interface ILessonRepo : IGenericRepo<Lesson>
{
    Task<IEnumerable<Lesson>> GetBySubjectIdAsync(int subjectId);
}
