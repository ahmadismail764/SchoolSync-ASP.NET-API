using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class TermService(ITermRepo termRepo) : GenericService<Term>(termRepo), ITermService
{
    private readonly ITermRepo _termRepo = termRepo;

    public async Task<IEnumerable<Term>> GetBySchoolYearAsync(int schoolYearId) => await _termRepo.GetBySchoolYearAsync(schoolYearId);
}
