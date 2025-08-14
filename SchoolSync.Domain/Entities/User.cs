namespace SchoolSync.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;

    public string? PasswordHash { get; set; }

    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; } = true;

    public int RoleId { get; set; } = 1;
    public int SchoolId { get; set; }
    public School School { get; set; } = null!;

    public Role Role { get; set; } = null!;

    public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public List<Subject> Subjects { get; set; } = new List<Subject>();
  
    public StudentDetails? Details { get; set; }
}