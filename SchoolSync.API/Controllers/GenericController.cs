using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.IServices;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenericController<TEntity, TDto, TCreateDto, TUpdateDto> : ControllerBase
    where TEntity : class
{
    private readonly IGenericService<TEntity> _service;
    private readonly IMapper _mapper;

    public GenericController(IGenericService<TEntity> service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<TDto>>> GetAll()
    {
        var entities = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<TDto>>(entities));
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<TDto>> GetById(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(_mapper.Map<TDto>(entity));
    }

    [HttpPost]
    public virtual async Task<ActionResult<TDto>> Create([FromBody] TCreateDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        var created = await _service.CreateAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = (created as dynamic).Id }, _mapper.Map<TDto>(created));
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(int id, [FromBody] TUpdateDto dto)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        _mapper.Map(dto, entity);
        await _service.UpdateAsync(entity);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
