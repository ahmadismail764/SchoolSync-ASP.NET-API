using AutoMapper;
using SchoolSync.Domain.Entities;
using SchoolSync.App.DTOs.Organization;

namespace SchoolSync.App.MappingProfiles;

public class OrganizationProfile : Profile
{
    public OrganizationProfile()
    {
        CreateMap<Organization, OrganizationDto>().ReverseMap();
        CreateMap<Organization, CreateOrganizationDto>().ReverseMap();
        CreateMap<Organization, UpdateOrganizationDto>().ReverseMap();
    }
}
