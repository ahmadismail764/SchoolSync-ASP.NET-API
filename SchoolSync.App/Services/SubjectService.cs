using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class SubjectService(ISubjectRepo subjectRepo) : GenericService<Subject>(subjectRepo), ISubjectService
{
    private readonly ISubjectRepo _subjectRepo = subjectRepo;

    public async Task<IEnumerable<Subject>> GetBySchoolAsync(int schoolId) => await _subjectRepo.GetBySchoolAsync(schoolId);
    public async Task<IEnumerable<Subject>> GetByTeacherAsync(int teacherId) => await _subjectRepo.GetByTeacherAsync(teacherId);
    public override Task ValidateAsync(Subject entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Name))
            throw new ArgumentException("Subject name is required.");
        if (string.IsNullOrWhiteSpace(entity.Code))
            throw new ArgumentException("Subject code is required.");
        if (entity.Credits < 1)
            throw new ArgumentException("Credits must be at least 1.");
        if (entity.SchoolId <= 0)
            throw new ArgumentException("SchoolId must be set.");
        if (entity.TeacherId <= 0)
            throw new ArgumentException("TeacherId must be set.");
        return Task.CompletedTask;
    }
}
