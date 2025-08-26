using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.School;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/schools")]
[Authorize(Roles = "2")]
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
        try
        {
            var entity = _mapper.Map<School>(dto);
            var created = await _service.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<SchoolDto>(created));
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
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSchoolDto dto)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        if (dto.Name != null) entity.Name = dto.Name;
        if (dto.Address != null) entity.Address = dto.Address;
        if (dto.PhoneNumber != null) entity.PhoneNumber = dto.PhoneNumber;
        if (dto.Email != null) entity.Email = dto.Email;
        if (dto.OrganizationId.HasValue) entity.OrganizationId = dto.OrganizationId.Value;
        if (dto.IsActive.HasValue) entity.IsActive = dto.IsActive.Value;
        try
        {
            await _service.UpdateAsync(entity);
            return Ok(_mapper.Map<SchoolDto>(entity));
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

    [HttpPut("range")]
    public async Task<ActionResult<IEnumerable<SchoolDto>>> UpdateRange([FromBody] UpdateSchoolDto dto, [FromQuery] string? nameContains = null)
    {
        try
        {
            var patch = new School();
            if (dto.Name != null) patch.Name = dto.Name;
            if (dto.Address != null) patch.Address = dto.Address;
            if (dto.PhoneNumber != null) patch.PhoneNumber = dto.PhoneNumber;
            if (dto.Email != null) patch.Email = dto.Email;
            if (dto.OrganizationId.HasValue) patch.OrganizationId = dto.OrganizationId.Value;
            if (dto.IsActive.HasValue) patch.IsActive = dto.IsActive.Value;
            var updated = await _service.UpdateRangeWhereAsync(
                school => string.IsNullOrEmpty(nameContains) || school.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase),
                patch
            );
            return Ok(_mapper.Map<IEnumerable<SchoolDto>>(updated));
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


    [HttpDelete("range")]
    public async Task<ActionResult<IEnumerable<SchoolDto>>> DeleteRange([FromQuery] string? nameContains = null)
    {
        try
        {
            var deleted = await _service.DeleteRangeWhereAsync(school => string.IsNullOrEmpty(nameContains) || school.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase));
            return Ok(_mapper.Map<IEnumerable<SchoolDto>>(deleted));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}
