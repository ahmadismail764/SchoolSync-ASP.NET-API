using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class SchoolYearService(ISchoolYearRepo schoolYearRepo) : GenericService<SchoolYear>(schoolYearRepo), ISchoolYearService
{
    private readonly ISchoolYearRepo _schoolYearRepo = schoolYearRepo;

    public async Task<IEnumerable<SchoolYear>> GetBySchoolAsync(int schoolId) => await _schoolYearRepo.GetBySchoolAsync(schoolId);
    public override Task ValidateAsync(SchoolYear entity)
    {
        if (entity.Year < 2000 || entity.Year > DateTime.UtcNow.Year + 1)
            throw new ArgumentException("Year is out of valid range.");
        if (entity.StartDate >= entity.EndDate)
            throw new ArgumentException("StartDate must be before EndDate.");
        if (entity.SchoolId <= 0)
            throw new ArgumentException("SchoolId must be set.");
        return Task.CompletedTask;
    }
}
