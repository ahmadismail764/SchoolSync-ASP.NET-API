using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.School;

public class CreateSchoolDto
{
    [Required, StringLength(100)]
    public string Name { get; set; } = null!;

    [Required, StringLength(200)]
    public string Address { get; set; } = null!;

    [Required, Phone, StringLength(20)]
    public string PhoneNumber { get; set; } = null!;

    [Required, EmailAddress, StringLength(100)]
    public string Email { get; set; } = null!;

    [Required]
    public int OrganizationId { get; set; }
}
