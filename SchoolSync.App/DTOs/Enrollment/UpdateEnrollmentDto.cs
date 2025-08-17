namespace SchoolSync.App.DTOs.Enrollment;

public class UpdateEnrollmentDto
{
    public int? StudentId { get; set; }
    public int? SubjectId { get; set; }
    public int? TermId { get; set; }
    public DateTime? EnrollmentDate { get; set; }
    public decimal? Grade { get; set; }
    public bool? IsActive { get; set; }
}
