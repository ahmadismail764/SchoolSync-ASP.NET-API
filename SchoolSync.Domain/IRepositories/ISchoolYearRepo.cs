using SchoolSync.Domain.Entities;
namespace SchoolSync.Domain.IRepositories;

public interface ISchoolYearRepo : IGenericRepo<SchoolYear>
{
    Task<IEnumerable<SchoolYear>> GetBySchoolAsync(int schoolId);
}
