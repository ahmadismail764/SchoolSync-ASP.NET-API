using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.Subject;

public class UpdateSubjectDto
{
    /// <summary>Name of the subject.</summary>
    [StringLength(100)]
    public string? Name { get; set; }

    /// <summary>Subject code.</summary>
    [StringLength(20)]
    public string? Code { get; set; }

    /// <summary>Number of credits for the subject.</summary>
    [Range(1, 20)]
    public int? Credits { get; set; }

    /// <summary>ID of the school this subject belongs to.</summary>
    public int? SchoolId { get; set; }

    /// <summary>ID of the teacher for this subject.</summary>
    public int? TeacherId { get; set; }

    /// <summary>Whether the subject is active.</summary>
    public bool? IsActive { get; set; }
}
