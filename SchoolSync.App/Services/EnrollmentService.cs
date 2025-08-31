using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class EnrollmentService(IEnrollmentRepo enrollmentRepo, IUserRepo userRepo, ISubjectRepo subjectRepo, ITermRepo termRepo, ISchoolYearRepo schoolYearRepo)
    : GenericService<Enrollment>(enrollmentRepo), IEnrollmentService
{
    private readonly IUserRepo _userRepo = userRepo;
    private readonly ISubjectRepo _subjectRepo = subjectRepo;
    private readonly ITermRepo _termRepo = termRepo;
    private readonly ISchoolYearRepo _schoolYearRepo = schoolYearRepo;

    public async Task<IEnumerable<Enrollment>> GetByStudentAsync(int studentId) => await enrollmentRepo.GetByStudentAsync(studentId);
    public async Task<IEnumerable<Enrollment>> GetBySubjectAsync(int subjectId) => await enrollmentRepo.GetBySubjectAsync(subjectId);
    public async Task<IEnumerable<Enrollment>> GetByTermAsync(int termId) => await enrollmentRepo.GetByTermAsync(termId);
    public async Task<Enrollment?> GetByCompositeKeyAsync(int studentId, int subjectId, int termId) => await enrollmentRepo.GetByCompositeKeyAsync(studentId, subjectId, termId);

}
