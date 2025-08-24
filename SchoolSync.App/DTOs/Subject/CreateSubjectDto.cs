using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.Subject;

public class CreateSubjectDto
{
    /// <summary>Name of the subject.</summary>
    [Required, StringLength(100)]
    public string Name { get; set; } = null!;

    /// <summary>Subject code.</summary>
    [Required, StringLength(20)]
    public string Code { get; set; } = null!;

    /// <summary>Number of credits for the subject.</summary>
    [Range(1, 20)]
    public int Credits { get; set; } = 3;

    /// <summary>ID of the school this subject belongs to.</summary>
    [Required]
    public int SchoolId { get; set; }

    /// <summary>ID of the teacher for this subject.</summary>
    [Required]
    public int TeacherId { get; set; }
}
