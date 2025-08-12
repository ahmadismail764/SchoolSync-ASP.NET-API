using SchoolSync.Domain.Entities;
namespace SchoolSync.Domain.IRepositories;

public interface ISchoolRepo : IGenericRepo<School>
{
    Task<School?> GetByNameAsync(int id);
    Task<School?> GetByOrganizationAsync(string name);    
}
