using SchoolSync.App.DTOs.School;
namespace SchoolSync.App.DTOs.Organization;

public class OrganizationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<SchoolDto> Schools { get; set; } = new();
}
