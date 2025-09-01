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

    public override async Task ValidateCreateAsync(SchoolYear entity)
    {
        if (entity.SchoolId <= 0)
            throw new ArgumentException("SchoolId must be set.");
        if (entity.StartDate >= entity.EndDate)
            throw new ArgumentException("StartDate must be before EndDate.");
        // Uniqueness: Year already exists per School
        if (await _schoolYearRepo.ExistsAsync(x => x.Year == entity.Year && x.SchoolId == entity.SchoolId))
            throw new ArgumentException("A school year with this year already exists in the school.", nameof(entity.Year));
    }

    public override async Task ValidateUpdateAsync(SchoolYear entity)
    {
        if (entity.SchoolId <= 0)
            throw new ArgumentException("SchoolId must be set.");
        if (entity.StartDate >= entity.EndDate)
            throw new ArgumentException("StartDate must be before EndDate.");
        // Uniqueness: Year already exists per School (exclude self)
        if (await _schoolYearRepo.ExistsAsync(x => x.Year == entity.Year && x.SchoolId == entity.SchoolId && x.Id != entity.Id))
            throw new ArgumentException("A school year with this year already exists in the school.", nameof(entity.Year));
    }
}
