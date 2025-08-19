using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class TermService(ITermRepo termRepo) : GenericService<Term>(termRepo), ITermService
{
    private readonly ITermRepo _termRepo = termRepo;

    public async Task<IEnumerable<Term>> GetBySchoolYearAsync(int schoolYearId) => await _termRepo.GetBySchoolYearAsync(schoolYearId);
    public override Task ValidateAsync(Term entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Name))
            throw new ArgumentException("Term name is required.");
        if (entity.StartDate >= entity.EndDate)
            throw new ArgumentException("StartDate must be before EndDate.");
        if (entity.SchoolYearId <= 0)
            throw new ArgumentException("SchoolYearId must be set.");
        return Task.CompletedTask;
    }
}
