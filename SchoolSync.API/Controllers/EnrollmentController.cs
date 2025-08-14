using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.Enrollment;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class EnrollmentController(IEnrollmentService service, IMapper mapper)
    : GenericController<Enrollment, EnrollmentDto, CreateEnrollmentDto, UpdateEnrollmentDto>(service, mapper)
{ }
