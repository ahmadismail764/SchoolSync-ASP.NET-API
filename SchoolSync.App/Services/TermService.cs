using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class TermService(ITermRepo termRepo, ISchoolYearRepo schoolYearRepo)
    : GenericService<Term>(termRepo), ITermService
{
    private readonly ITermRepo _termRepo = termRepo;
    private readonly ISchoolYearRepo _schoolYearRepo = schoolYearRepo;

    public async Task<IEnumerable<Term>> GetBySchoolYearAsync(int schoolYearId) => await _termRepo.GetBySchoolYearAsync(schoolYearId);
    public override async Task ValidateCreateAsync(Term entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Name))
            throw new ArgumentException("Term name is required.");
        if (entity.StartDate >= entity.EndDate)
            throw new ArgumentException("StartDate must be before EndDate.");
        if (entity.SchoolYearId <= 0)
            throw new ArgumentException("SchoolYearId must be set.");
        // Cross-entity: School year must exist, be active, and match term's school
        var schoolYear = await _schoolYearRepo.GetAsync(entity.SchoolYearId);
        if (schoolYear == null || !schoolYear.IsActive)
            throw new ArgumentException("School year must exist and be active.");
        // Optionally, check that the term's school matches the school year (if Term has SchoolId)
    }

    public override async Task ValidateUpdateAsync(Term entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Name))
            throw new ArgumentException("Term name is required.");
        if (entity.StartDate >= entity.EndDate)
            throw new ArgumentException("StartDate must be before EndDate.");
        if (entity.SchoolYearId <= 0)
            throw new ArgumentException("SchoolYearId must be set.");
        // Cross-entity: School year must exist, be active, and match term's school
        var schoolYear = await _schoolYearRepo.GetAsync(entity.SchoolYearId);
        if (schoolYear == null || !schoolYear.IsActive)
            throw new ArgumentException("School year must exist and be active.");
        // Optionally, check that the term's school matches the school year (if Term has SchoolId)
    }
}
