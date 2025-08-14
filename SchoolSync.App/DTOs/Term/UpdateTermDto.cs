namespace SchoolSync.App.DTOs.Term;

public class UpdateTermDto
{
    public string? Name { get; set; }
    public int? SchoolYearId { get; set; }
    public bool? IsActive { get; set; }
}
