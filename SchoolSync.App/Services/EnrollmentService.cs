using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class EnrollmentService(IEnrollmentRepo enrollmentRepo) : GenericService<Enrollment>(enrollmentRepo), IEnrollmentService
{
    private readonly IEnrollmentRepo _enrollmentRepo = enrollmentRepo;

    public async Task<IEnumerable<Enrollment>> GetByStudentAsync(int studentId) => await _enrollmentRepo.GetByStudentAsync(studentId);
    public async Task<IEnumerable<Enrollment>> GetBySubjectAsync(int subjectId) => await _enrollmentRepo.GetBySubjectAsync(subjectId);
}
