using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.Enrollment;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentController(IEnrollmentService service, IMapper mapper)
    : GenericController<Enrollment, EnrollmentDto, CreateEnrollmentDto, UpdateEnrollmentDto>(service, mapper)
{ }
