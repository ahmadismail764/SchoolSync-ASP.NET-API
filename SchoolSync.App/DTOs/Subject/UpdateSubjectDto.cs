namespace SchoolSync.App.DTOs.Subject;

public class UpdateSubjectDto
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public int? Credits { get; set; }
    public int? SchoolId { get; set; }
    public int? TeacherId { get; set; }
    public bool? IsActive { get; set; }
}
