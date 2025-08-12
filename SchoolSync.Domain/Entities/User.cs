namespace SchoolSync.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; } = true;

    public string Role { get; set; } = "Student";
    public int SchoolId { get; set; }
    public School School { get; set; } = null!;

    public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public StudentDetails? Details { get; set; }
}