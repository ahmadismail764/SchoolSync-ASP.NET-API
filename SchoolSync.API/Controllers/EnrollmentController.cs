using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.Enrollment;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "2")]
public class EnrollmentController(IEnrollmentService enrollmentService, IMapper mapper) : ControllerBase
{
    private readonly IEnrollmentService _service = enrollmentService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<EnrollmentDto>>> GetAll()
    {
        var entities = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<EnrollmentDto>>(entities));
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<EnrollmentDto>> GetById(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(_mapper.Map<EnrollmentDto>(entity));
    }

    [HttpPost]
    public virtual async Task<ActionResult<EnrollmentDto>> Create([FromBody] CreateEnrollmentDto dto)
    {
        try
        {
            var entity = _mapper.Map<Enrollment>(dto);
            var created = await _service.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = (created as dynamic).Id }, _mapper.Map<EnrollmentDto>(created));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }


    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(int id, [FromBody] UpdateEnrollmentDto dto)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        if (dto.StudentId.HasValue) entity.StudentId = dto.StudentId.Value;
        if (dto.SubjectId.HasValue) entity.SubjectId = dto.SubjectId.Value;
        if (dto.TermId.HasValue) entity.TermId = dto.TermId.Value;
        if (dto.EnrollmentDate.HasValue) entity.EnrollmentDate = dto.EnrollmentDate.Value;
        if (dto.Grade.HasValue) entity.Grade = dto.Grade.Value;
        if (dto.IsActive.HasValue) entity.IsActive = dto.IsActive.Value;
        try
        {
            await _service.UpdateAsync(entity);
            return Ok(_mapper.Map<EnrollmentDto>(entity));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
    // [HttpPut("range")]
    // public async Task<IActionResult> UpdateRange([FromBody] UpdateEnrollmentDto dto, [FromQuery] string? nameContains = null)
    // {
    //     var enrollment = new Organization();        
    //     _mapper.Map(dto, enrollment);
    //     await _service.UpdateRangeWhereAsync(
    //         org => string.IsNullOrEmpty(nameContains) || enrollment.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase),
    //         enrollment
    //     );
    //     return NoContent();
    // }

    // [HttpDelete("range")]
    // public async Task<IActionResult> DeleteRange([FromQuery] string? nameContains = null)
    // {
    //     try
    //     {
    //         await _service.DeleteRangeWhereAsync(org => string.IsNullOrEmpty(nameContains) || org.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase));
    //     }
    //     catch (Exception ex)
    //     {
    //         return StatusCode(500, $"An error occurred: {ex.Message}");
    //     }
    //     return NoContent();
    // }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
