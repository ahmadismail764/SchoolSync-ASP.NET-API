using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.School;

public class UpdateSchoolDto
{
    [StringLength(100)]
    public string? Name { get; set; }
    [StringLength(200)]
    public string? Address { get; set; }
    [Phone, StringLength(20)]
    public string? PhoneNumber { get; set; }
    [EmailAddress, StringLength(100)]
    public string? Email { get; set; }
    public int? OrganizationId { get; set; }
}
