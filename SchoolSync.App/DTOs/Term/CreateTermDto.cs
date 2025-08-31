using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.Term;

public class CreateTermDto
{
    [Required, StringLength(50)]
    public string Name { get; set; } = null!;
    [Required]
    public DateTime StartDate { get; set; } = DateTime.Now;
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    public int SchoolYearId { get; set; }
    public bool IsActive { get; set; } = true;
}
