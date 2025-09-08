namespace SchoolSync.App.DTOs.Enrollment;

public class EnrollmentDto
{
    public int UserId { get; set; }
    public int TermId { get; set; }
    public int SubjectId { get; set; }
    public decimal? Grade { get; set; }
}
