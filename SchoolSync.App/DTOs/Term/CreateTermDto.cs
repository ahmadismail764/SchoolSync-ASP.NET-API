namespace SchoolSync.App.DTOs.Term;

public class CreateTermDto
{
    public string Name { get; set; } = string.Empty;
    public int SchoolYearId { get; set; }
}
