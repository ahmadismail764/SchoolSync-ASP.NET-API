using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.School;

namespace SchoolSync.API.Controllers;

[Authorize(Roles = "2")]
[ApiController]
[Route("api/[controller]")]

// Require authentication for all endpoints in this controller
[Authorize]
public class SchoolController(ISchoolService service, IMapper mapper) : ControllerBase
{
    private readonly ISchoolService _service = service;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SchoolDto>>> GetAll()
    {
        var entities = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<SchoolDto>>(entities));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SchoolDto>> GetById(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(_mapper.Map<SchoolDto>(entity));
    }

    [HttpGet("range")]
    public async Task<ActionResult<IEnumerable<SchoolDto>>> GetRange([FromQuery] string? nameContains = null)
    {
        var entities = await _service.GetRangeWhereAsync(
            school => string.IsNullOrEmpty(nameContains) || school.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase)
        );
        return Ok(_mapper.Map<IEnumerable<SchoolDto>>(entities));
    }

    [HttpPost]
    public async Task<ActionResult<SchoolDto>> Create([FromBody] CreateSchoolDto dto)
    {
        var entity = _mapper.Map<School>(dto);
        var created = await _service.CreateAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<SchoolDto>(created));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSchoolDto dto)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        _mapper.Map(dto, entity);
        await _service.UpdateAsync(entity);
        try
        {
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpPut("range")]
    public async Task<IActionResult> UpdateRange([FromBody] UpdateSchoolDto dto, [FromQuery] string? nameContains = null)
    {
        var entity = new School();        
        _mapper.Map(dto, entity);
        await _service.UpdateRangeWhereAsync(
            school => string.IsNullOrEmpty(nameContains) || school.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase),
            entity
        );
        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        await _service.DeleteAsync(id);
        return NoContent();
    }


    [HttpDelete("range")]
    public async Task<IActionResult> DeleteRange([FromQuery] string? nameContains = null)
    {
        try
        {
            await _service.DeleteRangeWhereAsync(school => string.IsNullOrEmpty(nameContains) || school.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
        return NoContent();
    }
}
