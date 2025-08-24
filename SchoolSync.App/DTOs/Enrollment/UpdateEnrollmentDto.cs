using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.Enrollment;

public class UpdateEnrollmentDto
{
    /// <summary>ID of the student being enrolled.</summary>
    public int? StudentId { get; set; }

    /// <summary>ID of the subject for enrollment.</summary>
    public int? SubjectId { get; set; }

    /// <summary>ID of the term for enrollment.</summary>
    public int? TermId { get; set; }

    /// <summary>Date of enrollment.</summary>
    public DateTime? EnrollmentDate { get; set; }

    /// <summary>Grade for the enrollment.</summary>
    public decimal? Grade { get; set; }

    /// <summary>Whether the enrollment is active.</summary>
    public bool? IsActive { get; set; }
}
