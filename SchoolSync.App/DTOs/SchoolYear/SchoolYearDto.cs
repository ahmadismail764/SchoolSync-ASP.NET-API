namespace SchoolSync.App.DTOs.SchoolYear;

public class SchoolYearDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int SchoolId { get; set; }
    public bool IsActive { get; set; }
}
