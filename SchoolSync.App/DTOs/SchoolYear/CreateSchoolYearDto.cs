using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.SchoolYear;

public class CreateSchoolYearDto
{
    /// <summary>Name of the school year.</summary>
    [Required, StringLength(50)]
    public string Name { get; set; } = null!;

    /// <summary>Year (e.g., 2025).</summary>
    [Required, Range(2000, 2100)]
    public int Year { get; set; }

    /// <summary>Start date of the school year.</summary>
    [Required]
    public DateTime StartDate { get; set; }

    /// <summary>End date of the school year.</summary>
    [Required]
    public DateTime EndDate { get; set; }

    /// <summary>ID of the school this year belongs to.</summary>
    [Required]
    public int SchoolId { get; set; }
}
