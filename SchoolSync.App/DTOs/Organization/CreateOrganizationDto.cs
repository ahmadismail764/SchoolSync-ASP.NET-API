using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.Organization;

public class CreateOrganizationDto
{
    /// <summary>Name of the organization.</summary>
    [Required, StringLength(100)]
    public string Name { get; set; } = null!;

    /// <summary>Description of the organization.</summary>
    [StringLength(500)]
    public string Description { get; set; } = null!;

    /// <summary>Organization address.</summary>
    [Required, StringLength(200)]
    public string Address { get; set; } = null!;

    /// <summary>Organization phone number.</summary>
    [Required, Phone, StringLength(20)]
    public string PhoneNumber { get; set; } = null!;

    /// <summary>Organization email address.</summary>
    [Required, EmailAddress, StringLength(100)]
    public string Email { get; set; } = null!;
}
