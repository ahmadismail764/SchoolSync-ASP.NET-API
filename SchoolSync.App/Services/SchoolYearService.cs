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
}
