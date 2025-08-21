namespace SchoolSync.App.DTOs.School;

public class CreateSchoolDto
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int OrganizationId { get; set; }
}
