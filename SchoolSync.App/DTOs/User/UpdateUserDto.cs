using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.User;

public class UpdateUserDto
{
    /// <summary>Full name of the user.</summary>
    [StringLength(100)]
    public string? FullName { get; set; }

    /// <summary>User's email address.</summary>
    [EmailAddress, StringLength(100)]
    public string? Email { get; set; }

    /// <summary>Unique username for login.</summary>
    [StringLength(50)]
    public string? Username { get; set; }

    /// <summary>User's password (plain, will be hashed).</summary>
    [StringLength(100)]
    public string? Password { get; set; }

    /// <summary>Optional phone number.</summary>
    [Phone]
    public string? PhoneNumber { get; set; }

    /// <summary>Whether the user is active.</summary>
    public bool? IsActive { get; set; }

    /// <summary>School identifier.</summary>
    public int? SchoolId { get; set; }

    /// <summary>Role identifier.</summary>
    public int? RoleId { get; set; }
}
