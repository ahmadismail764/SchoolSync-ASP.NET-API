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
        // 1. Validate Student exists and is a student
        var student = await _userRepo.GetAsync(entity.StudentId) ?? throw new ArgumentException("Student not found.");
        if (student.RoleId != 1) // Assuming 2 is Student role
            throw new ArgumentException("User is not a student.");

        // 2. Validate Subject exists
        var subject = await _subjectRepo.GetAsync(entity.SubjectId) ?? throw new ArgumentException("Subject not found.");

        // 3. Validate Term exists
        var term = await _termRepo.GetAsync(entity.TermId) ?? throw new ArgumentException("Term not found.");

        // 4. Validate Student and Subject belong to the same school
        if (student.SchoolId != subject.SchoolId)
            throw new ArgumentException("Student and subject must belong to the same school.");

        // 5. Validate Term belongs to the student's school
        var schoolYear = await _schoolYearRepo.GetAsync(term.SchoolYearId);
        if (schoolYear == null || schoolYear.SchoolId != student.SchoolId)
            throw new ArgumentException("Term must belong to the student's school.");

        // 6. Uniqueness: a student can only be enrolled once per subject per term
        if (await enrollmentRepo.ExistsAsync(e => e.StudentId == entity.StudentId && e.SubjectId == entity.SubjectId && e.TermId == entity.TermId))
            throw new ArgumentException("This student is already enrolled in this subject for this term.");
    }
    public override async Task ValidateUpdateAsync(Enrollment entity)
    {
        // Apply same validations as create for data integrity
        await ValidateCreateAsync(entity);

        // Additional check: Uniqueness excluding self
        if (await enrollmentRepo.ExistsAsync(e => e.StudentId == entity.StudentId && e.SubjectId == entity.SubjectId && e.TermId == entity.TermId && e.Id != entity.Id))
            throw new ArgumentException("This student is already enrolled in this subject for this term.");
    }
}
