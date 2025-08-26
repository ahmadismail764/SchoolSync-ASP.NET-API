using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.SchoolYear;

public class CreateSchoolYearDto
{

    [Required, Range(2000, 2100)]
    public int Year { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public int SchoolId { get; set; }
}
