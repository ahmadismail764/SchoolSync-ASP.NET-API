using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.Term;

public class CreateTermDto
{
    /// <summary>Name of the term.</summary>
    [Required, StringLength(50)]
    public string Name { get; set; } = null!;

    /// <summary>Start date of the term.</summary>
    [Required]
    public DateTime StartDate { get; set; } = DateTime.Now;

    /// <summary>End date of the term.</summary>
    [Required]
    public DateTime EndDate { get; set; }

    /// <summary>ID of the school year this term belongs to.</summary>
    [Required]
    public int SchoolYearId { get; set; }

    /// <summary>Whether the term is active.</summary>
    public bool IsActive { get; set; } = true;
}
