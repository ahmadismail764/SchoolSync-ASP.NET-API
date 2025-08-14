using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.Organization;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrganizationController(IOrganizationService service, IMapper mapper)
    : GenericController<Organization, OrganizationDto, CreateOrganizationDto, UpdateOrganizationDto>(service, mapper)
{ }
