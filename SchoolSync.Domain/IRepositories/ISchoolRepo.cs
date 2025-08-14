using SchoolSync.Domain.Entities;
namespace SchoolSync.Domain.IRepositories;

public interface ISchoolRepo : IGenericRepo<School>
{
    Task<School?> GetByNameAsync(string name);
    Task<School?> GetByOrganizationAsync(int orgId);
}
