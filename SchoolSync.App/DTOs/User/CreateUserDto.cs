namespace SchoolSync.App.DTOs.User;

public class CreateUserDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public int SchoolId { get; set; }
    public int RoleId { get; set; }
}
