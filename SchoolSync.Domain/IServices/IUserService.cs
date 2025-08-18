using SchoolSync.Domain.Entities;

namespace SchoolSync.Domain.IServices;

public interface IUserService : IGenericService<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetByRoleAsync(int roleId);
    Task<IEnumerable<User>> GetBySchoolAsync(int schoolId);
    Task<User?> GetStudentWithDetailsAsync(int studentId);
    Task<IEnumerable<User>> GetAllStudentsWithDetailsAsync();
    Task<bool> ValidatePasswordAsync(User user, string password);

    Task<User?> AuthenticateAsync(string username, string password);
}
