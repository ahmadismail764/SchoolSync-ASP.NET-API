using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.Enrollment;

public class CreateEnrollmentDto
{
    /// <summary>ID of the student being enrolled.</summary>
    [Required]
    public int StudentId { get; set; }

    /// <summary>ID of the subject for enrollment.</summary>
    [Required]
    public int SubjectId { get; set; }

    /// <summary>ID of the term for enrollment.</summary>
    [Required]
    public int TermId { get; set; }
}
