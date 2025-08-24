using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class UserService(IUserRepo userRepo, IEnrollmentRepo enrollmentRepo)
    : GenericService<User>(userRepo), IUserService
{
    private readonly IEnrollmentRepo _enrollmentRepo = enrollmentRepo;

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
    public override async Task<User> CreateAsync(User entity)
    {
        await ValidateAsync(entity);
        if (!string.IsNullOrEmpty(entity.PasswordHash))
            entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(entity.PasswordHash);
        return await base.CreateAsync(entity);
    }

    public override async Task ValidateAsync(User entity)
    {
        if (string.IsNullOrWhiteSpace(entity.FullName))
            throw new ArgumentException("Full name is required.");
        if (string.IsNullOrWhiteSpace(entity.Username))
            throw new ArgumentException("Username is required.");
        if (string.IsNullOrWhiteSpace(entity.Email) || !entity.Email.Contains('@'))
            throw new ArgumentException("Valid email is required.");
        if (entity.RoleId <= 0)
            throw new ArgumentException("RoleId must be set.");
        if (entity.SchoolId <= 0)
            throw new ArgumentException("SchoolId must be set.");

        var existingByUsername = await userRepo.GetByUsernameAsync(entity.Username);
        if (existingByUsername != null)
            throw new ArgumentException("Username already exists.");

        var existingByEmail = await userRepo.GetByEmailAsync(entity.Email);
        if (existingByEmail != null && existingByEmail.Id != entity.Id)
            throw new ArgumentException("Email already exists.");
    }

    public override async Task DeleteAsync(int id)
    {
        var user = await _repo.GetAsync(id);
        if (user == null || !user.IsActive) return;

        // Deactivate enrollments
        var enrollments = await _enrollmentRepo.GetRangeWhereAsync(e => e.StudentId == user.Id && e.IsActive);
        foreach (var enrollment in enrollments)
        {
            enrollment.IsActive = false;
            await _enrollmentRepo.UpdateAsync(enrollment);
        }
        user.IsActive = false;
        await _repo.UpdateAsync(user);
        await _repo.SaveChangesAsync();
    }
}
