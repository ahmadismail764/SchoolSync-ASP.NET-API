namespace SchoolSync.Domain.Entities;

public class StudentDetails
{
    public int StudentId { get; set; }
    public decimal? GPA { get; set; }
    public decimal? attendanceRate { get; set; }
    public User Student { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}
