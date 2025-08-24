using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;

namespace SchoolSync.App.Services;

public class OrganizationService(IOrganizationRepo organizationRepo, ISchoolRepo schoolRepo, IUserRepo userRepo, ISubjectRepo subjectRepo, ISchoolYearRepo schoolYearRepo)
    : GenericService<Organization>(organizationRepo), IOrganizationService
{
    private readonly ISchoolRepo _schoolRepo = schoolRepo;
    private readonly IUserRepo _userRepo = userRepo;
    private readonly ISubjectRepo _subjectRepo = subjectRepo;
    private readonly ISchoolYearRepo _schoolYearRepo = schoolYearRepo;

    public override async Task ValidateAsync(Organization entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Name))
            throw new ArgumentException("Organization name is required.");
        if (string.IsNullOrWhiteSpace(entity.Email) || !entity.Email.Contains('@'))
            throw new ArgumentException("Valid organization email is required.");

        // Prevent deactivation if active schools exist
        if (!entity.IsActive)
        {
            var activeSchools = await _schoolRepo.GetRangeWhereAsync(s => s.OrganizationId == entity.Id && s.IsActive);
            if (activeSchools.Any())
                throw new InvalidOperationException("Cannot deactivate organization with active schools. Deactivate schools first.");
        }
    }

    public override async Task DeleteAsync(int id)
    {
        var org = await organizationRepo.GetAsync(id);
        if (org == null || !org.IsActive) return;

        // Deactivate all related schools and their children
        var schools = await _schoolRepo.GetRangeWhereAsync(s => s.OrganizationId == id && s.IsActive);
        foreach (var school in schools)
        {
            // Deactivate users
            var users = await _userRepo.GetRangeWhereAsync(u => u.SchoolId == school.Id && u.IsActive);
            foreach (var user in users)
            {
                user.IsActive = false;
                await _userRepo.UpdateAsync(user);
            }
            // Deactivate subjects
            var subjects = await _subjectRepo.GetRangeWhereAsync(sub => sub.SchoolId == school.Id && sub.IsActive);
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
            await _schoolRepo.UpdateAsync(school);
        }
        org.IsActive = false;
        await organizationRepo.UpdateAsync(org);
        await organizationRepo.SaveChangesAsync();
    }
}
