using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.Term;

public class UpdateTermDto
{
    /// <summary>Name of the term.</summary>
    [StringLength(50)]
    public string? Name { get; set; }

    /// <summary>Start date of the term.</summary>
    public DateTime? StartDate { get; set; }

    /// <summary>End date of the term.</summary>
    public DateTime? EndDate { get; set; }

    /// <summary>ID of the school year this term belongs to.</summary>
    public int? SchoolYearId { get; set; }

    /// <summary>Whether the term is active.</summary>
    public bool? IsActive { get; set; }
}
