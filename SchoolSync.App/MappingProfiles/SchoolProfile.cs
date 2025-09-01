using AutoMapper;
using SchoolSync.Domain.Entities;
using SchoolSync.App.DTOs.School;

namespace SchoolSync.App.MappingProfiles;

public class SchoolProfile : Profile
{
    public SchoolProfile()
    {
        CreateMap<School, SchoolDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.OrganizationId, opt => opt.MapFrom(src => src.OrganizationId));

        CreateMap<SchoolDto, School>()
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        CreateMap<School, CreateSchoolDto>().ReverseMap();
        CreateMap<School, UpdateSchoolDto>().ReverseMap();
    }
}
