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
}
