using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.Organization;

public class UpdateOrganizationDto
{
    /// <summary>Name of the organization.</summary>
    [StringLength(100)]
    public string? Name { get; set; }

    /// <summary>Organization address.</summary>
    [StringLength(200)]
    public string? Address { get; set; }

    /// <summary>Whether the organization is active.</summary>
    public bool? IsActive { get; set; }
}
