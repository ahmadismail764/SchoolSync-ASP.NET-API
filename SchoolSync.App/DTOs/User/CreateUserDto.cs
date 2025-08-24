using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.User;

public class CreateUserDto
{
    /// <summary>Full name of the user.</summary>
    [Required, StringLength(100)]
    public string FullName { get; set; } = null!;

    /// <summary>Unique username for login.</summary>
    [Required, StringLength(50)]
    public string Username { get; set; } = null!;

    /// <summary>User's password (plain, will be hashed).</summary>
    [Required, StringLength(100)]
    public string Password { get; set; } = null!;

    /// <summary>User's email address.</summary>
    [Required, EmailAddress, StringLength(100)]
    public string Email { get; set; } = null!;

    /// <summary>Optional phone number.</summary>
    [Phone]
    public string? PhoneNumber { get; set; }

    /// <summary>Role ID (e.g., 1=Student, 2=Teacher, etc.).</summary>
    [Required]
    public int RoleId { get; set; } = 1;

    /// <summary>School ID the user belongs to.</summary>
    [Required]
    public int SchoolId { get; set; }
}
