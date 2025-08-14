using SchoolSync.Domain.Entities;

namespace SchoolSync.Domain.IServices;

public interface ITermService : IGenericService<Term>
{
    Task<IEnumerable<Term>> GetBySchoolYearAsync(int schoolYearId);
}
