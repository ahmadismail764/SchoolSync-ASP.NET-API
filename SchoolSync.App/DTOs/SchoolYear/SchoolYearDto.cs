namespace SchoolSync.App.DTOs.SchoolYear;

public class SchoolYearDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int SchoolId { get; set; }
    public bool IsActive { get; set; }
    public SchoolSync.App.DTOs.School.SchoolDto? School { get; set; }
    public List<SchoolSync.App.DTOs.Term.TermDto> Terms { get; set; } = new();
}
