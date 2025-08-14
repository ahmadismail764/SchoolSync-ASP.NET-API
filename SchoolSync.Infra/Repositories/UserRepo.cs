using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.Entities;
using SchoolSync.Infra.Persistence;
using Microsoft.EntityFrameworkCore;
namespace SchoolSync.Infra.Repositories;

public class UserRepo(DBContext context) : GenericRepo<User>(context), IUserRepo
{
    public async Task<User?> GetByUsernameAsync(string userName)
    {
        var users = await GetRangeWhereAsync(u => u.Username == userName);
        return users.FirstOrDefault();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var users = await GetRangeWhereAsync(u => u.Email == email);
        return users.FirstOrDefault();
    }
    public async Task<IEnumerable<User>> GetByRoleAsync(int roleId)
    {
        return await GetRangeWhereAsync(u => u.RoleId == roleId);
    }
    public async Task<IEnumerable<User>> GetBySchoolAsync(int schoolId)
    {
        return await GetRangeWhereAsync(u => u.SchoolId == schoolId);
    }
   

    public async Task<IEnumerable<User>> GetAllStudentsWithDetailsAsync()
    {
        return await context.Set<User>()
            .Include(u => u.Role)
            .Include(u => u.Details)
            .Where(u => u.RoleId == 2) // Assuming 2 is the Student role ID
            .ToListAsync();
    }
    public async Task<User?> GetStudentWithDetailsAsync(int studentId)
    {
        return await context.Set<User>()
            .Include(u => u.Role)
            .Include(u => u.Details)
            .FirstOrDefaultAsync(u => u.Id == studentId && u.RoleId == 2); // Assuming 2 is the Student role ID
    }

}
