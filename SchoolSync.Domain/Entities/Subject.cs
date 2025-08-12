namespace SchoolSync.Domain.Entities;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int Credits { get; set; } = 3;
    public int SchoolId { get; set; }
    public int TeacherId { get; set; }
    public bool IsActive { get; set; } = true;

    public User Teacher { get; set; } = null!;
    public School School { get; set; } = null!;
    public List<Enrollment> Enrollments { get; set; } = new();
}