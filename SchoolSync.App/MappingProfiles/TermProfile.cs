using AutoMapper;
using SchoolSync.Domain.Entities;
using SchoolSync.App.DTOs.Term;

namespace SchoolSync.App.MappingProfiles;

public class TermProfile : Profile
{
    public TermProfile()
    {
        CreateMap<Term, TermDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
            .ForMember(dest => dest.SchoolYearId, opt => opt.MapFrom(src => src.SchoolYearId));

        CreateMap<TermDto, Term>()
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        CreateMap<Term, CreateTermDto>().ReverseMap();
        CreateMap<Term, UpdateTermDto>().ReverseMap();
    }
}
