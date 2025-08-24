using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.School;

public class CreateSchoolDto
{
    /// <summary>Name of the school.</summary>
    [Required, StringLength(100)]
    public string Name { get; set; } = null!;

    /// <summary>School address.</summary>
    [Required, StringLength(200)]
    public string Address { get; set; } = null!;

    /// <summary>School phone number.</summary>
    [Required, Phone, StringLength(20)]
    public string PhoneNumber { get; set; } = null!;

    /// <summary>School email address.</summary>
    [Required, EmailAddress, StringLength(100)]
    public string Email { get; set; } = null!;

    /// <summary>ID of the organization this school belongs to.</summary>
    [Required]
    public int OrganizationId { get; set; }
}
