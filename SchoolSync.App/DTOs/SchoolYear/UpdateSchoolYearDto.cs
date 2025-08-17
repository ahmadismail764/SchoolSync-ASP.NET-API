namespace SchoolSync.App.DTOs.SchoolYear;

public class UpdateSchoolYearDto
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? SchoolId { get; set; }
    public bool? IsActive { get; set; }
}
