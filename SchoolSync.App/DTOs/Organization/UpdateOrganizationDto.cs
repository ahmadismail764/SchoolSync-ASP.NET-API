using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.Organization;

public class UpdateOrganizationDto
{
    [StringLength(100)]
    public string? Name { get; set; }

    [Phone]
    [StringLength(20)]
    public string? Phone { get; set; }

    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }
}
