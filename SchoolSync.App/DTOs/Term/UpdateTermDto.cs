using System.ComponentModel.DataAnnotations;

namespace SchoolSync.App.DTOs.Term;

public class UpdateTermDto
{
    [StringLength(50)]
    public string? Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? SchoolYearId { get; set; }
}
