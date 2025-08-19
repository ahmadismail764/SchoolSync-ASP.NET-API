using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class EnrollmentService(IEnrollmentRepo enrollmentRepo) : GenericService<Enrollment>(enrollmentRepo), IEnrollmentService
{
    private readonly IEnrollmentRepo _enrollmentRepo = enrollmentRepo;

    public async Task<IEnumerable<Enrollment>> GetByStudentAsync(int studentId) => await _enrollmentRepo.GetByStudentAsync(studentId);
    public async Task<IEnumerable<Enrollment>> GetBySubjectAsync(int subjectId) => await _enrollmentRepo.GetBySubjectAsync(subjectId);
    public override Task ValidateAsync(Enrollment entity)
    {
        if (entity.StudentId <= 0)
            throw new ArgumentException("StudentId must be set.");
        if (entity.SubjectId <= 0)
            throw new ArgumentException("SubjectId must be set.");
        if (entity.TermId <= 0)
            throw new ArgumentException("TermId must be set.");
        if (entity.EnrollmentDate == default)
            throw new ArgumentException("EnrollmentDate is required.");
        return Task.CompletedTask;
    }
}
