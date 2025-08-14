namespace SchoolSync.App.DTOs.User;

public class UpdateUserDto
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public int? SchoolId { get; set; }
    public int? RoleId { get; set; }
    public bool? IsActive { get; set; }
}
