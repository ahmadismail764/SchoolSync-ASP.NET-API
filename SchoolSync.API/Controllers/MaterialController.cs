using Microsoft.AspNetCore.Mvc;
using SchoolSync.API.Helpers;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/materials")]
public class MaterialController(IMaterialService materialService) : ControllerBase
{
    private readonly IMaterialService _materialService = materialService;

    [HttpGet("{id}/download")]
    public async Task<IActionResult> Download(int id)
    {
        var material = await _materialService.GetByIdAsync(id);
        if (material == null)
            return NotFound(new { description = $"Material with ID {id} not found." });
        if (material.FileData == null || material.FileData.Length == 0)
            return NotFound(new { description = "File data is missing for this material." });
        return File(material.FileData, material.ContentType, material.FileName);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] int lessonId, [FromForm] string? description, [FromForm] IFormFile file)
    {
        // Check null-ness
        if (file == null || file.Length == 0)
            return BadRequest(new { description = "No file uploaded." });

        // Now actually handle it
        UploadResult uploadResult;
        if (file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
            uploadResult = await UploadHandler.HandlePdfUpload(file);
        else if (file.ContentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase))
            uploadResult = await UploadHandler.HandleVideoUpload(file);
        else
            return BadRequest(new { description = "Unsupported file type. Only PDF and video files are allowed." });

        // Something bad happened?
        if (!uploadResult.Success)
            return BadRequest(new { description = uploadResult.ErrorMessage ?? "File upload failed." });

        var material = new Material
        {
            LessonId = lessonId,
            FileName = uploadResult.FileName!,
            ContentType = uploadResult.ContentType!,
            FileSize = uploadResult.FileData?.Length ?? 0,
            FileData = uploadResult.FileData!,
            Description = description
        };
        await _materialService.CreateAsync(material);
        return NoContent();
    }
}
