using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class UserService(IUserRepo userRepo)
    : GenericService<User>(userRepo), IUserService
{

    public async Task<User?> GetByUsernameAsync(string username) => await userRepo.GetByUsernameAsync(username);
    public async Task<User?> GetByEmailAsync(string email) => await userRepo.GetByEmailAsync(email);
    public async Task<IEnumerable<User>> GetByRoleAsync(int roleId) => await userRepo.GetByRoleAsync(roleId);
    public async Task<IEnumerable<User>> GetBySchoolAsync(int schoolId) => await userRepo.GetBySchoolAsync(schoolId);
    public async Task<User?> GetStudentWithDetailsAsync(int studentId) => await userRepo.GetAsync(studentId);
    public async Task<IEnumerable<User>> GetAllStudentsWithDetailsAsync() => await userRepo.GetByRoleAsync(2);

    public async Task<bool> ValidatePasswordAsync(User user, string password)
    {
        if (user == null || string.IsNullOrEmpty(user.PasswordHash))
            return false;
        return await Task.FromResult(BCrypt.Net.BCrypt.Verify(password, user.PasswordHash));
    }
    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        var user = await GetByUsernameAsync(username);
        if (user == null)
            return null;

        var isValid = await ValidatePasswordAsync(user, password);
        return isValid ? user : null;
    }
}
