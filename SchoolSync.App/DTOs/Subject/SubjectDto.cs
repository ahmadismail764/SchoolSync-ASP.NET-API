namespace SchoolSync.App.DTOs.Subject;

public class SubjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int SchoolId { get; set; }
    public int TeacherId { get; set; }
    public bool IsActive { get; set; }
}
