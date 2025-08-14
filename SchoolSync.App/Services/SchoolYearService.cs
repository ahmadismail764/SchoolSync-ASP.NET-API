using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class SchoolYearService(ISchoolYearRepo schoolYearRepo) : GenericService<SchoolYear>(schoolYearRepo), ISchoolYearService
{
    private readonly ISchoolYearRepo _schoolYearRepo = schoolYearRepo;

    public async Task<IEnumerable<SchoolYear>> GetBySchoolAsync(int schoolId) => await _schoolYearRepo.GetBySchoolAsync(schoolId);
}
