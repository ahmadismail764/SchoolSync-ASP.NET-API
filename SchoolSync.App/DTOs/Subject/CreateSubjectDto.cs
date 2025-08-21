namespace SchoolSync.App.DTOs.Subject;

public class CreateSubjectDto
{
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public int Credits { get; set; } = 3;
    public int SchoolId { get; set; }
    public int TeacherId { get; set; }
}
