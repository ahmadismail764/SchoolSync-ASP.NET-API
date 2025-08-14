using SchoolSync.Domain.Entities;

namespace SchoolSync.Domain.IServices;

public interface ISchoolYearService : IGenericService<SchoolYear>
{
    Task<IEnumerable<SchoolYear>> GetBySchoolAsync(int schoolId);
}
