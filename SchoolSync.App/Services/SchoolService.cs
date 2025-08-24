using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class SchoolService(ISchoolRepo schoolRepo, IUserRepo userRepo, ISubjectRepo subjectRepo, ISchoolYearRepo schoolYearRepo, IOrganizationRepo organizationRepo)
    : GenericService<School>(schoolRepo), ISchoolService
{
    private readonly IUserRepo _userRepo = userRepo;
    private readonly ISubjectRepo _subjectRepo = subjectRepo;
    private readonly ISchoolYearRepo _schoolYearRepo = schoolYearRepo;
    private readonly IOrganizationRepo _organizationRepo = organizationRepo;

    public async Task<School?> GetByOrganizationAsync(int orgId) => await schoolRepo.GetByOrganizationAsync(orgId);
    public async Task<School?> GetByNameAsync(string name) => await schoolRepo.GetByNameAsync(name);

    public override async Task ValidateAsync(School entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Name))
            throw new ArgumentException("School name is required.");
        if (string.IsNullOrWhiteSpace(entity.Email) || !entity.Email.Contains('@'))
            throw new ArgumentException("Valid school email is required.");
        if (entity.OrganizationId <= 0)
            throw new ArgumentException("OrganizationId must be set.");
        // Cross-entity: Organization must exist and be active
        var org = await _organizationRepo.GetAsync(entity.OrganizationId);
        if (org == null || !org.IsActive)
            throw new ArgumentException("Organization must exist and be active.");
        // Prevent deactivation if active children exist
        if (!entity.IsActive)
        {
            var activeUsers = await _userRepo.GetRangeWhereAsync(u => u.SchoolId == entity.Id && u.IsActive);
            if (activeUsers.Any())
                throw new InvalidOperationException("Cannot deactivate school with active users. Deactivate users first.");
            var activeSubjects = await _subjectRepo.GetRangeWhereAsync(s => s.SchoolId == entity.Id && s.IsActive);
            if (activeSubjects.Any())
                throw new InvalidOperationException("Cannot deactivate school with active subjects. Deactivate subjects first.");
            var activeYears = await _schoolYearRepo.GetRangeWhereAsync(y => y.SchoolId == entity.Id && y.IsActive);
            if (activeYears.Any())
                throw new InvalidOperationException("Cannot deactivate school with active school years. Deactivate school years first.");
        }
    }

    public override async Task DeleteAsync(int id)
    {
        var school = await _repo.GetAsync(id);
        if (school == null || !school.IsActive) return;

        // Deactivate users
        var users = await _userRepo.GetRangeWhereAsync(u => u.SchoolId == school.Id && u.IsActive);
        foreach (var user in users)
        {
            user.IsActive = false;
            await _userRepo.UpdateAsync(user);
        }
        // Deactivate subjects
        var subjects = await _subjectRepo.GetRangeWhereAsync(s => s.SchoolId == school.Id && s.IsActive);
        foreach (var subject in subjects)
        {
            subject.IsActive = false;
            await _subjectRepo.UpdateAsync(subject);
        }
        // Deactivate school years
        var years = await _schoolYearRepo.GetRangeWhereAsync(y => y.SchoolId == school.Id && y.IsActive);
        foreach (var year in years)
        {
            year.IsActive = false;
            await _schoolYearRepo.UpdateAsync(year);
        }
        school.IsActive = false;
        await _repo.UpdateAsync(school);
        await _repo.SaveChangesAsync();
    }
}
