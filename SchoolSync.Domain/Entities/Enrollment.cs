namespace SchoolSync.Domain.Entities;

/// <summary>
/// Central junction entity representing a student's enrollment in a subject for a specific term
/// Tracks enrollment date, grades, and active status
/// Unique constraint: one student can only enroll once per subject per term
/// </summary>
public class Enrollment
{
    public int Id { get; set; }
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
