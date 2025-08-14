using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class SubjectService(ISubjectRepo subjectRepo) : GenericService<Subject>(subjectRepo), ISubjectService
{
    private readonly ISubjectRepo _subjectRepo = subjectRepo;

    public async Task<IEnumerable<Subject>> GetBySchoolAsync(int schoolId) => await _subjectRepo.GetBySchoolAsync(schoolId);
    public async Task<IEnumerable<Subject>> GetByTeacherAsync(int teacherId) => await _subjectRepo.GetByTeacherAsync(teacherId);
}
