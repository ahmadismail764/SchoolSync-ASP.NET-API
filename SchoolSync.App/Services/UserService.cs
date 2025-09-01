using Microsoft.AspNetCore.Identity;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class UserService(IUserRepo userRepo, IPasswordHasher<User> passwordHasher)
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
        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return await Task.FromResult(result == PasswordVerificationResult.Success);
    }

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        var user = await GetByUsernameAsync(username);
        if (user == null)
            return null;

        var isValid = await ValidatePasswordAsync(user, password);
        return isValid ? user : null;
    }

    public override async Task ValidateCreateAsync(User entity)
    {
        // Username uniqueness
        if (await userRepo.ExistsAsync(u => u.Username == entity.Username))
            throw new ArgumentException("Username already exists.");

        // Email uniqueness
        if (await userRepo.ExistsAsync(u => u.Email == entity.Email))
            throw new ArgumentException("Email already exists.");

        // Phone number uniqueness (if provided)
        if (!string.IsNullOrWhiteSpace(entity.PhoneNumber))
        {
            if (await userRepo.ExistsAsync(u => u.PhoneNumber == entity.PhoneNumber))
                throw new ArgumentException("Phone number already exists.");
        }
    }

    public override async Task ValidateUpdateAsync(User entity)
    {
        // Username uniqueness (exclude self)
        if (await userRepo.ExistsAsync(u => u.Username == entity.Username && u.Id != entity.Id))
            throw new ArgumentException("Username already exists.");

        // Email uniqueness (exclude self)
        if (await userRepo.ExistsAsync(u => u.Email == entity.Email && u.Id != entity.Id))
            throw new ArgumentException("Email already exists.");
        // Phone number uniqueness (if provided, exclude self)
        if (!string.IsNullOrWhiteSpace(entity.PhoneNumber))
        {
            if (await userRepo.ExistsAsync(u => u.Id != entity.Id && u.PhoneNumber == entity.PhoneNumber))
                throw new ArgumentException("Phone number already exists.");
        }
    }
}
