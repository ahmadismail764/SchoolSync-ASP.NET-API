namespace SchoolSync.App.DTOs.School;

public class UpdateSchoolDto
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public bool? IsActive { get; set; }
}
