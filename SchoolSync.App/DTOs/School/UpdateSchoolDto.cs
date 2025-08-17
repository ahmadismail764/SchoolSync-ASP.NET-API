namespace SchoolSync.App.DTOs.School;

public class UpdateSchoolDto
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public int? OrganizationId { get; set; }
    public bool? IsActive { get; set; }
}
