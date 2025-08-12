using SchoolSync.Domain.Entities;
namespace SchoolSync.Domain.IRepositories;

public interface ISchoolYearRepo
{
    Task<IEnumerable<SchoolYear>> GetBySchoolAsync(int schoolId);
}
