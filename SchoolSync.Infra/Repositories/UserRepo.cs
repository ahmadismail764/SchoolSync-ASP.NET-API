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

    public async Task<User?> GetWithRoleAsync(int id)
    {
        var users = await GetRangeWhereAsync(u => u.Id == id);
        return users.FirstOrDefault();
    }
}
