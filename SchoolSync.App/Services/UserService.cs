using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;
namespace SchoolSync.App.Services;

public class UserService(IUserRepo userRepo) : GenericService<User>(userRepo), IUserService
{
    private readonly IUserRepo _userRepo = userRepo;

    public async Task<User?> GetByUsernameAsync(string username) => await _userRepo.GetByUsernameAsync(username);
    public async Task<User?> GetByEmailAsync(string email) => await _userRepo.GetByEmailAsync(email);
    public async Task<IEnumerable<User>> GetByRoleAsync(int roleId) => await _userRepo.GetByRoleAsync(roleId);
    public async Task<IEnumerable<User>> GetBySchoolAsync(int schoolId) => await _userRepo.GetBySchoolAsync(schoolId);
    public async Task<User?> GetStudentWithDetailsAsync(int studentId) => await _userRepo.GetStudentWithDetailsAsync(studentId);
    public async Task<IEnumerable<User>> GetAllStudentsWithDetailsAsync() => await _userRepo.GetAllStudentsWithDetailsAsync();
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
    public override async Task<User> CreateAsync(User entity)
    {
        await ValidateAsync(entity);
        if (!string.IsNullOrEmpty(entity.PasswordHash))
            entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(entity.PasswordHash);
        return await base.CreateAsync(entity);
    }

    public override Task ValidateAsync(User entity)
    {
        if (string.IsNullOrWhiteSpace(entity.FullName))
            throw new ArgumentException("Full name is required.");
        if (string.IsNullOrWhiteSpace(entity.Username))
            throw new ArgumentException("Username is required.");
        if (string.IsNullOrWhiteSpace(entity.Email) || !entity.Email.Contains("@"))
            throw new ArgumentException("Valid email is required.");
        if (entity.RoleId <= 0)
            throw new ArgumentException("RoleId must be set.");
        if (entity.SchoolId <= 0)
            throw new ArgumentException("SchoolId must be set.");
        return Task.CompletedTask;
    }
}
