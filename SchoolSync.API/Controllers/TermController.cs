using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.Term;

namespace SchoolSync.API.Controllers;

[Authorize(Roles = "2")]
[ApiController]
[Route("api/[controller]")]
public class TermController(ITermService service, IMapper mapper) : ControllerBase
{
    private readonly ITermService _service = service;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TermDto>>> GetAll()
    {
        var entities = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<TermDto>>(entities));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TermDto>> GetById(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(_mapper.Map<TermDto>(entity));
    }

    [HttpGet("range")]
    public async Task<ActionResult<IEnumerable<TermDto>>> GetRange([FromQuery] string? nameContains = null)
    {
        var entities = await _service.GetRangeWhereAsync(
            term => string.IsNullOrEmpty(nameContains) || term.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase)
        );
        return Ok(_mapper.Map<IEnumerable<TermDto>>(entities));
    }

    [HttpPost]
    public async Task<ActionResult<TermDto>> Create([FromBody] CreateTermDto dto)
    {
        var entity = _mapper.Map<Term>(dto);
        var created = await _service.CreateAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<TermDto>(created));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTermDto dto)
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        await _service.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("range")]
    public async Task<IActionResult> UpdateRange([FromBody] UpdateTermDto dto, [FromQuery] string? nameContains = null)
    {
        var entity = new Term();
        _mapper.Map(dto, entity);
        await _service.UpdateRangeWhereAsync(
            term => string.IsNullOrEmpty(nameContains) || term.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase),
            entity
        );
        return NoContent();
    }

    [HttpDelete("range")]
    public async Task<IActionResult> DeleteRange([FromQuery] string? nameContains = null)
    {
        try
        {
            await _service.DeleteRangeWhereAsync(org => string.IsNullOrEmpty(nameContains) || org.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
        return NoContent();
    }
}

