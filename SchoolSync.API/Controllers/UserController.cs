using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.User;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService service, IMapper mapper)
    : GenericController<User, UserDto, CreateUserDto, UpdateUserDto>(service, mapper)
{ }
