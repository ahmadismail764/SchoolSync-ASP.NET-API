using SchoolSync.Domain.Entities;
namespace SchoolSync.Domain.IRepositories;

public interface IUserRepo : IGenericRepo<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetByRoleAsync(int roleId);
    Task<IEnumerable<User>> GetBySchoolAsync(int schoolId);
}
