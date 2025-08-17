using SchoolSync.App.DTOs.Subject;
namespace SchoolSync.App.DTOs.User;

public class UserDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public int SchoolId { get; set; }
    public int RoleId { get; set; }
    public bool IsActive { get; set; }

    public List<SubjectDto> Subjects { get; set; } = new();
}
