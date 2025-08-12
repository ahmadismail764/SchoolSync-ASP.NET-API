using SchoolSync.Domain.Entities;
namespace SchoolSync.Domain.IRepositories;

public interface ITermRepo : IGenericRepo<Term>
{
    Task<IEnumerable<Term>> GetBySchoolYearAsync(int schoolYearId);
}
