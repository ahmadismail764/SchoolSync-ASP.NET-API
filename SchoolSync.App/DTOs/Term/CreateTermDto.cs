namespace SchoolSync.App.DTOs.Term;

public class CreateTermDto
{
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; }
    public int SchoolYearId { get; set; }
    public bool IsActive { get; set; } = true;
}
