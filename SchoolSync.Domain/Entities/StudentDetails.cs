namespace SchoolSync.Domain.Entities;

public class StudentDetails
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public decimal? GPA { get; set; }
    public decimal? AttendanceRate { get; set; }
    public decimal? ParticipationRating { get; set; }
    public User Student { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}
