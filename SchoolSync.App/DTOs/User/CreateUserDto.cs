using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.User;

public class CreateUserDto
{
    [Required, StringLength(100)]
    public string FullName { get; set; } = null!;
    [Required, StringLength(50)]
    public string Username { get; set; } = null!;
    [Required, StringLength(100)]
    public string Password { get; set; } = null!;
    [Required, EmailAddress, StringLength(100)]
    public string Email { get; set; } = null!;
    [Phone]
    public string? PhoneNumber { get; set; }
    [Required]
    public int RoleId { get; set; } = 1;
    [Required]
    public int SchoolId { get; set; }
}
