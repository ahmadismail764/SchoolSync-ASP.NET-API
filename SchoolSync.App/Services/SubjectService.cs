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
}
