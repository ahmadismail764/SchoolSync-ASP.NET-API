namespace SchoolSync.App.DTOs.Term;

public class UpdateTermDto
{
    public string? Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? SchoolYearId { get; set; }
    public bool? IsActive { get; set; }
}
