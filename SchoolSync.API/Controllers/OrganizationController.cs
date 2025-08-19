using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.Organization;

namespace SchoolSync.API.Controllers;

[Authorize(Roles = "2")]
[ApiController]
[Route("api/[controller]")]
// Require authentication for all endpoints in this controller
[Authorize]
public class OrganizationController(IOrganizationService service, IMapper mapper) : ControllerBase
{
    private readonly IOrganizationService _service = service;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrganizationDto>>> GetAll()
    {
        var entities = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<OrganizationDto>>(entities));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrganizationDto>> GetById(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(_mapper.Map<OrganizationDto>(entity));
    }

    [HttpGet("range")]
    public async Task<ActionResult<IEnumerable<OrganizationDto>>> GetRange([FromQuery] string? nameContains = null)
    {
        var entities = await _service.GetRangeWhereAsync(
            org => string.IsNullOrEmpty(nameContains) || org.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase)
        );
        return Ok(_mapper.Map<IEnumerable<OrganizationDto>>(entities));
    }

    [HttpPost]
    public async Task<ActionResult<OrganizationDto>> Create([FromBody] CreateOrganizationDto dto)
    {
        var entity = _mapper.Map<Organization>(dto);
        var created = await _service.CreateAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<OrganizationDto>(created));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateOrganizationDto dto)
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
    public async Task<IActionResult> UpdateRange([FromBody] UpdateOrganizationDto dto, [FromQuery] string? nameContains = null)
    {
        var entity = new Organization();
        _mapper.Map(dto, entity);
        await _service.UpdateRangeWhereAsync(
            org => string.IsNullOrEmpty(nameContains) || org.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase),
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
