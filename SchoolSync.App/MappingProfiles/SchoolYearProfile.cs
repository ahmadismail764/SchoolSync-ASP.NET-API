using AutoMapper;
using SchoolSync.Domain.Entities;
using SchoolSync.App.DTOs.SchoolYear;

namespace SchoolSync.App.MappingProfiles;

public class SchoolYearProfile : Profile
{
    public SchoolYearProfile()
    {
        CreateMap<SchoolYear, SchoolYearDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
            .ForMember(dest => dest.SchoolId, opt => opt.MapFrom(src => src.SchoolId));

        CreateMap<SchoolYearDto, SchoolYear>()
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        CreateMap<SchoolYear, CreateSchoolYearDto>().ReverseMap();
        CreateMap<SchoolYear, UpdateSchoolYearDto>().ReverseMap();
    }
}
