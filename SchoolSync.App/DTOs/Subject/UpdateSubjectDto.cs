namespace SchoolSync.App.DTOs.Subject;

public class UpdateSubjectDto
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public int? SchoolId { get; set; }
    public bool? IsActive { get; set; }
}
