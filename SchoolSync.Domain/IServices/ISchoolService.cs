using SchoolSync.Domain.Entities;

namespace SchoolSync.Domain.IServices;

public interface ISchoolService : IGenericService<School>
{
    Task<School?> GetByNameAsync(string name);
    Task<School?> GetByOrganizationAsync(int orgId);
}
