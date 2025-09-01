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

    public override async Task ValidateCreateAsync(Enrollment entity)
    {
        // Uniqueness: a student can only be enrolled once per subject per term
        if (await enrollmentRepo.ExistsAsync(e => e.StudentId == entity.StudentId && e.SubjectId == entity.SubjectId && e.TermId == entity.TermId))
            throw new ArgumentException("This student is already enrolled in this subject for this term.");
    }

    public override async Task ValidateUpdateAsync(Enrollment entity)
    {
        // Uniqueness: a student can only be enrolled once per subject per term (exclude self)
        if (await enrollmentRepo.ExistsAsync(e => e.StudentId == entity.StudentId && e.SubjectId == entity.SubjectId && e.TermId == entity.TermId && e.Id != entity.Id))
            throw new ArgumentException("This student is already enrolled in this subject for this term.");
    }
}
