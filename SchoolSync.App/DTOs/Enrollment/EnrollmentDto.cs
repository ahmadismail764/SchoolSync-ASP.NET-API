namespace SchoolSync.App.DTOs.Enrollment;

public class EnrollmentDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int SchoolId { get; set; }
    public int SchoolYearId { get; set; }
    public int TermId { get; set; }
    public int SubjectId { get; set; }
    public bool IsActive { get; set; }
    public SchoolSync.App.DTOs.User.UserDto? User { get; set; }
    public SchoolSync.App.DTOs.School.SchoolDto? School { get; set; }
    public SchoolSync.App.DTOs.SchoolYear.SchoolYearDto? SchoolYear { get; set; }
    public SchoolSync.App.DTOs.Term.TermDto? Term { get; set; }
    public SchoolSync.App.DTOs.Subject.SubjectDto? Subject { get; set; }
}
