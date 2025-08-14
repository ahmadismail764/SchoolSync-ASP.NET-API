using SchoolSync.Domain.Entities;

namespace SchoolSync.Domain.IServices;

public interface ISchoolService : IGenericService<School>
{
    Task<School?> GetByOrganizationAsync(int orgId);
    Task<School?> GetByNameAsync(string name);
}
