using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.SchoolYear;

namespace SchoolSync.API.Controllers;

[Authorize(Roles = "2")]
[ApiController]
[Route("api/[controller]")]
// Require authentication for all endpoints in this controller
public class SchoolYearController(ISchoolYearService service, IMapper mapper) : ControllerBase
{
    private readonly ISchoolYearService _service = service;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SchoolYearDto>>> GetAll()
    {
        var entities = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<SchoolYearDto>>(entities));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SchoolYearDto>> GetById(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(_mapper.Map<SchoolYearDto>(entity));
    }

    [HttpPost]
    public async Task<ActionResult<SchoolYearDto>> Create([FromBody] CreateSchoolYearDto dto)
    {
        try
        {
            var entity = _mapper.Map<SchoolYear>(dto);
            var created = await _service.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<SchoolYearDto>(created));
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
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSchoolYearDto dto)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        if (dto.StartDate.HasValue) entity.StartDate = dto.StartDate.Value;
        if (dto.EndDate.HasValue) entity.EndDate = dto.EndDate.Value;
        if (dto.SchoolId.HasValue) entity.SchoolId = dto.SchoolId.Value;
        if (dto.IsActive.HasValue) entity.IsActive = dto.IsActive.Value;
        try
        {
            await _service.UpdateAsync(entity);
            return Ok(_mapper.Map<SchoolYearDto>(entity));
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        await _service.DeleteAsync(id);
        return NoContent();
    }
    // [HttpPut("range")]
    // public async Task<IActionResult> UpdateRange([FromBody] UpdateSchoolYearDto dto, [FromQuery] string? nameContains = null)
    // {
    //     var entity = new SchoolYear();        
    //     _mapper.Map(dto, entity);
    //     await _service.UpdateRangeWhereAsync(
    //         schlyear => string.IsNullOrEmpty(nameContains) || schlyear.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase),
    //         entity
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
}
