using SchoolSync.Domain.Entities;

namespace SchoolSync.Domain.IServices;

public interface IStudentDetailsService : IGenericService<StudentDetails>
{
    Task<StudentDetails?> GetByStudentAsync(int studentId);
}
