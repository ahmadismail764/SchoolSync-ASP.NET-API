using SchoolSync.Domain.IServices;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.Entities;
namespace SchoolSync.App.Services;

public class LessonService(ILessonRepo repo) : GenericService<Lesson>(repo), ILessonService
{
    public async Task<IEnumerable<Lesson>> GetBySubjectAsync(int SubjectId) => await GetRangeWhereAsync(x => x.SubjectId == SubjectId);

    public override async Task ValidateCreateAsync(Lesson entity)
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

        // Uniqueness: Title must be unique per Subject
        var existing = await repo.GetRangeWhereAsync(x => x.Title == entity.Title && x.SubjectId == entity.SubjectId);
        if (existing.Any())
            throw new ArgumentException("A lesson with this title already exists for the subject.", nameof(entity.Title));
    }

    public override Task ValidateUpdateAsync(Lesson entity)
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
