namespace SchoolSync.Domain.IServices;

public interface ILessonService<T> : IGenericService<T> where T : class
{
    Task<IEnumerable<T>> GetBySubjectAsync(int SubjectId);
}
