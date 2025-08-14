namespace SchoolSync.App.DTOs.Enrollment;

public class CreateEnrollmentDto
{
    public int UserId { get; set; }
    public int SchoolId { get; set; }
    public int SchoolYearId { get; set; }
    public int TermId { get; set; }
    public int SubjectId { get; set; }
}
