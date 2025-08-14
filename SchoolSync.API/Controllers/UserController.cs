using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.User;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UserController : GenericController<User, UserDto, CreateUserDto, UpdateUserDto>
{
    private readonly IUserService _service;
    private readonly IMapper _mapper;

    public UserController(IUserService service, IMapper mapper)
        : base(service, mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public override async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto dto)
    {
        var entity = _mapper.Map<User>(dto);
        if (!string.IsNullOrEmpty(dto.Password))
        {
            entity.PasswordHash = dto.Password;
        }
        var created = await _service.CreateAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<UserDto>(created));
    }
}
