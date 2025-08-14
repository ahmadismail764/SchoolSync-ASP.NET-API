namespace SchoolSync.App.DTOs.SchoolYear;

public class SchoolYearDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int SchoolId { get; set; }
    public bool IsActive { get; set; }
}
