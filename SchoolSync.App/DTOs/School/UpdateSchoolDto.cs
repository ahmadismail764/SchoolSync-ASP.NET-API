using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.School;

public class UpdateSchoolDto
{
    /// <summary>Name of the school.</summary>
    [StringLength(100)]
    public string? Name { get; set; }

    /// <summary>School address.</summary>
    [StringLength(200)]
    public string? Address { get; set; }

    /// <summary>School phone number.</summary>
    [Phone, StringLength(20)]
    public string? PhoneNumber { get; set; }

    /// <summary>School email address.</summary>
    [EmailAddress, StringLength(100)]
    public string? Email { get; set; }

    /// <summary>Whether the school is active.</summary>
    public bool? IsActive { get; set; }

    /// <summary>Organization Id.</summary>
    public int? OrganizationId { get; set; }
}
