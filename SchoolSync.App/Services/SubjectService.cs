using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class SubjectService(ISubjectRepo subjectRepo, IEnrollmentRepo enrollmentRepo, IUserRepo userRepo)
    : GenericService<Subject>(subjectRepo), ISubjectService
{
    private readonly IEnrollmentRepo _enrollmentRepo = enrollmentRepo;
    private readonly IUserRepo _userRepo = userRepo;

    public async Task<IEnumerable<Subject>> GetBySchoolAsync(int schoolId) => await subjectRepo.GetBySchoolAsync(schoolId);
    public async Task<IEnumerable<Subject>> GetByTeacherAsync(int teacherId) => await subjectRepo.GetByTeacherAsync(teacherId);
    public override async Task ValidateAsync(Subject entity)
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
        // Cross-entity: Teacher must exist, be active, and be in the same school
        var teacher = await _userRepo.GetAsync(entity.TeacherId);
        if (teacher == null || !teacher.IsActive)
            throw new ArgumentException("Teacher must exist and be active.");
        if (teacher.SchoolId != entity.SchoolId)
            throw new ArgumentException("Teacher must be in the same school as the subject.");
    }

    public override async Task DeleteAsync(int id)
    {
        var subject = await _repo.GetAsync(id);
        if (subject == null || !subject.IsActive) return;

        // Deactivate enrollments
        var enrollments = await _enrollmentRepo.GetRangeWhereAsync(e => e.SubjectId == subject.Id && e.IsActive);
        foreach (var enrollment in enrollments)
        {
            enrollment.IsActive = false;
            await _enrollmentRepo.UpdateAsync(enrollment);
        }
        subject.IsActive = false;
        await _repo.UpdateAsync(subject);
        await _repo.SaveChangesAsync();
    }
}
