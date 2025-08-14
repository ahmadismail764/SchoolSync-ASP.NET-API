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
}
