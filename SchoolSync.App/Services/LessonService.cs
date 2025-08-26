using SchoolSync.Domain.IServices;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.Entities;
namespace SchoolSync.App.Services;

public class LessonService(ILessonRepo repo) : GenericService<Lesson>(repo), ILessonService<Lesson>
{
    public async Task<IEnumerable<Lesson>> GetBySubjectAsync(int SubjectId) => await GetRangeWhereAsync(x => x.SubjectId == SubjectId);

    public override Task ValidateAsync(Lesson entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Title))
            throw new ArgumentException("Title is required.", nameof(entity.Title));
        if (entity.Title.Length > 100)
            throw new ArgumentException("Title cannot exceed 100 characters.", nameof(entity.Title));
        if (string.IsNullOrWhiteSpace(entity.Description))
            throw new ArgumentException("Description is required.", nameof(entity.Description));
        if (entity.Description.Length > 500)
            throw new ArgumentException("Description cannot exceed 500 characters.", nameof(entity.Description));
        if (entity.SubjectId <= 0)
            throw new ArgumentException("SubjectId must be a positive integer.", nameof(entity.SubjectId));
        return Task.CompletedTask;
    }
}
