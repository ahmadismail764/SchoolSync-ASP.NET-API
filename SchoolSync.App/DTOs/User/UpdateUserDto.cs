using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.User;

public class UpdateUserDto
{
    [StringLength(100)]
    public string? FullName { get; set; }
    [EmailAddress, StringLength(100)]
    public string? Email { get; set; }
    [StringLength(50)]
    public string? Username { get; set; }
    [StringLength(100)]
    public string? Password { get; set; }
    [Phone]
    public string? PhoneNumber { get; set; }
    public int? SchoolId { get; set; }
    public int? RoleId { get; set; }
}
