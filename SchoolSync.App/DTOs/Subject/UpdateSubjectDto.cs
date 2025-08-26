using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.Subject;

public class UpdateSubjectDto
{
    [StringLength(100)]
    public string? Name { get; set; }
    [StringLength(20)]
    public string? Code { get; set; }
    [Range(1, 20)]
    public int? Credits { get; set; }
    public int? SchoolId { get; set; }

    public int? TeacherId { get; set; }
    public bool? IsActive { get; set; }
}
