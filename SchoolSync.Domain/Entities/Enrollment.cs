namespace SchoolSync.Domain.Entities;

public class Enrollment
{
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    public int TermId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public decimal? Grade { get; set; }
    public bool IsActive { get; set; } = true;

    public User Student { get; set; } = null!;
    public Subject Subject { get; set; } = null!;
    public Term Term { get; set; } = null!;
}
