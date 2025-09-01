using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class SubjectService(ISubjectRepo subjectRepo, IUserRepo userRepo)
    : GenericService<Subject>(subjectRepo), ISubjectService
{
    private readonly IUserRepo _userRepo = userRepo;

    public async Task<IEnumerable<Subject>> GetBySchoolAsync(int schoolId) => await subjectRepo.GetBySchoolAsync(schoolId);
    public async Task<IEnumerable<Subject>> GetByTeacherAsync(int teacherId) => await subjectRepo.GetByTeacherAsync(teacherId);

    public override async Task ValidateCreateAsync(Subject entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Name))
            throw new ArgumentException("Subject name is required.");
        if (entity.SchoolId <= 0)
            throw new ArgumentException("SchoolId must be set.");

        // Uniqueness: Name already exists per School
        if (await subjectRepo.ExistsAsync(x => x.Name == entity.Name && x.SchoolId == entity.SchoolId))
            throw new ArgumentException("A subject with this name already exists in the school.", nameof(entity.Name));
    }

    public override async Task ValidateUpdateAsync(Subject entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Name))
            throw new ArgumentException("Subject name is required.");
        if (entity.SchoolId <= 0)
            throw new ArgumentException("SchoolId must be set.");

        // Uniqueness: Name already exists per School (exclude self)
        if (await subjectRepo.ExistsAsync(x => x.Name == entity.Name && x.SchoolId == entity.SchoolId && x.Id != entity.Id))
            throw new ArgumentException("A subject with this name already exists in the school.", nameof(entity.Name));
    }
}
