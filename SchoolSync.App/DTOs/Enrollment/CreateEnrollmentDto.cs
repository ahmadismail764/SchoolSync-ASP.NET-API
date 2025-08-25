using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.Enrollment;

public class CreateEnrollmentDto
{
    [Required]
    public int StudentId { get; set; }

    [Required]
    public int SubjectId { get; set; }

    [Required]
    public int TermId { get; set; }
}
