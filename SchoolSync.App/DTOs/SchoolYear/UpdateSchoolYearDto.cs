using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.SchoolYear;

public class UpdateSchoolYearDto
{
    /// <summary>Name of the school year.</summary>
    [StringLength(50)]
    public string? Name { get; set; }

    /// <summary>Start date of the school year.</summary>
    public DateTime? StartDate { get; set; }

    /// <summary>End date of the school year.</summary>
    public DateTime? EndDate { get; set; }

    /// <summary>ID of the school this year belongs to.</summary>
    public int? SchoolId { get; set; }

    /// <summary>Whether the school year is active.</summary>
    public bool? IsActive { get; set; }
}
