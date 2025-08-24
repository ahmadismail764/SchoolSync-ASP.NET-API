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
    public override async Task ValidateAsync(Enrollment entity)
    {
        // Check user (student)
        var student = await _userRepo.GetAsync(entity.StudentId);
        if (student == null || !student.IsActive)
            throw new ArgumentException("Enrollment must be for an active student.");
        // Check subject
        var subject = await _subjectRepo.GetAsync(entity.SubjectId);
        if (subject == null || !subject.IsActive)
            throw new ArgumentException("Enrollment must be for an active subject.");
        // Check term
        var term = await _termRepo.GetAsync(entity.TermId);
        if (term == null || !term.IsActive)
            throw new ArgumentException("Enrollment must be for an active term.");
        // Check teacher
        var teacher = await _userRepo.GetAsync(subject.TeacherId);
        if (teacher == null || !teacher.IsActive)
            throw new ArgumentException("Subject must have an active teacher.");
        // Student and teacher must be in the same school
        if (student.SchoolId != teacher.SchoolId)
            throw new ArgumentException("Student and teacher must be in the same school.");
        // Subject and student must be in the same school
        if (subject.SchoolId != student.SchoolId)
            throw new ArgumentException("Subject and student must be in the same school.");
        // Subject and term must be in the same school (via school year)
        var schoolYear = await _schoolYearRepo.GetAsync(term.SchoolYearId);
        if (schoolYear == null || !schoolYear.IsActive)
            throw new ArgumentException("Term must be part of an active school year.");
        if (schoolYear.SchoolId != subject.SchoolId)
            throw new ArgumentException("Term's school year must match the subject's school.");
    }

    public override async Task DeleteAsync(int id)
    {
        var enrollment = await _repo.GetAsync(id);
        if (enrollment == null || !enrollment.IsActive) return;
        enrollment.IsActive = false;
        await _repo.UpdateAsync(enrollment);
        await _repo.SaveChangesAsync();
    }
}
