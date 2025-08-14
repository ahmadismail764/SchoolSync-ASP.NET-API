namespace SchoolSync.App.DTOs.Subject;

public class CreateSubjectDto
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int SchoolId { get; set; }
}
