namespace SchoolSync.App.DTOs.Term;

public class TermDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int SchoolYearId { get; set; }
    public bool IsActive { get; set; }
    public SchoolSync.App.DTOs.SchoolYear.SchoolYearDto? SchoolYear { get; set; }
}
