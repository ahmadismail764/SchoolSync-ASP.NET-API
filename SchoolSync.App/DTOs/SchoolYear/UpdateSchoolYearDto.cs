namespace SchoolSync.App.DTOs.SchoolYear;

public class UpdateSchoolYearDto
{
    public string? Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? SchoolId { get; set; }
    public bool? IsActive { get; set; }
}
