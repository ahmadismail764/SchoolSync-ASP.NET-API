namespace SchoolSync.App.DTOs.User;

public class CreateUserDto
{
    public string FullName { get; set; } = null!;
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; } = true;
    public int RoleId { get; set; } = 1;
    public int SchoolId { get; set; }
}
