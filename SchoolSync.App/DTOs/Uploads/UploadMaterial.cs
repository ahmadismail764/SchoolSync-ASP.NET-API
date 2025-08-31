using Microsoft.AspNetCore.Http;
namespace SchoolSync.App.DTOs.Uploads;
public class UploadMaterialDto
{
    public IFormFile File { get; set; } = null!;
}