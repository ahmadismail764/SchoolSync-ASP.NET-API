using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class SchoolYearService(ISchoolYearRepo schoolYearRepo, ITermRepo termRepo, ISchoolRepo schoolRepo)
    : GenericService<SchoolYear>(schoolYearRepo), ISchoolYearService
{
    private readonly ISchoolYearRepo _schoolYearRepo = schoolYearRepo;
    private readonly ITermRepo _termRepo = termRepo;
    private readonly ISchoolRepo _schoolRepo = schoolRepo;
    public async Task<IEnumerable<SchoolYear>> GetBySchoolAsync(int schoolId)
        => await _schoolYearRepo.GetRangeWhereAsync(y => y.SchoolId == schoolId);
    public override async Task ValidateAsync(SchoolYear entity)
    {
        if (entity.Year < 2000 || entity.Year > DateTime.UtcNow.Year + 1)
            throw new ArgumentException("Year is out of valid range.");
        if (entity.StartDate >= entity.EndDate)
            throw new ArgumentException("StartDate must be before EndDate.");
        if (entity.SchoolId <= 0)
            throw new ArgumentException("SchoolId must be set.");
        // Cross-entity: School must exist and be active
        var school = await _schoolRepo.GetAsync(entity.SchoolId);
        if (school == null || !school.IsActive)
            throw new ArgumentException("School must exist and be active.");
        // Prevent deactivation if active terms exist
        if (!entity.IsActive)
        {
            var activeTerms = await _termRepo.GetRangeWhereAsync(t => t.SchoolYearId == entity.Id && t.IsActive);
            if (activeTerms.Any())
                throw new InvalidOperationException("Cannot deactivate school year with active terms. Deactivate terms first.");

        }
    }
}
