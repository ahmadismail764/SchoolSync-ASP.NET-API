using SchoolSync.Domain.IServices;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.Entities;
namespace SchoolSync.App.Services;

public class LessonService(ILessonRepo repo, ISubjectRepo subjectRepo) : GenericService<Lesson>(repo), ILessonService
{
    private readonly ISubjectRepo _subjectRepo = subjectRepo;

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

        // Validate that the subject exists
        var subject = await _subjectRepo.GetAsync(entity.SubjectId);
        if (subject == null)
            throw new ArgumentException("Subject not found.", nameof(entity.SubjectId));

        // Uniqueness: Title already exists per Subject
        if (await repo.ExistsAsync(x => x.Title == entity.Title && x.SubjectId == entity.SubjectId))
            throw new ArgumentException("A lesson with this title already exists for the subject.", nameof(entity.Title));
    }

    public override async Task ValidateUpdateAsync(Lesson entity)
    {
        // Get the existing lesson to preserve SubjectId
        var existingLesson = await repo.GetAsync(entity.Id);
        if (existingLesson == null)
            throw new ArgumentException("Lesson not found.", nameof(entity.Id));

        // SubjectId should not be changeable during updates
        if (entity.SubjectId != existingLesson.SubjectId)
            throw new ArgumentException("SubjectId cannot be changed during update.", nameof(entity.SubjectId));

        // Validate Title if it's being updated (not null/empty)
        if (!string.IsNullOrWhiteSpace(entity.Title))
        {
            if (entity.Title.Length > 100)
                throw new ArgumentException("Title cannot exceed 100 characters.", nameof(entity.Title));

            // Check title uniqueness within the same subject (exclude self)
            if (await repo.ExistsAsync(x => x.Title == entity.Title && x.SubjectId == entity.SubjectId && x.Id != entity.Id))
                throw new ArgumentException("A lesson with this title already exists for the subject.", nameof(entity.Title));
        }

        // Validate Description if it's being updated (not null/empty)
        if (!string.IsNullOrWhiteSpace(entity.Description))
        {
            if (entity.Description.Length > 500)
                throw new ArgumentException("Description cannot exceed 500 characters.", nameof(entity.Description));
        }
    }
}
