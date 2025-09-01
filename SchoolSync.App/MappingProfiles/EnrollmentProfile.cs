using AutoMapper;
using SchoolSync.Domain.Entities;
using SchoolSync.App.DTOs.Enrollment;

namespace SchoolSync.App.MappingProfiles;

public class EnrollmentProfile : Profile
{
    public EnrollmentProfile()
    {
        CreateMap<Enrollment, EnrollmentDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.StudentId))
            .ForMember(dest => dest.SchoolId, opt => opt.MapFrom(src => src.Student.SchoolId))
            .ForMember(dest => dest.SchoolYearId, opt => opt.MapFrom(src => src.Term.SchoolYearId))
            .ForMember(dest => dest.TermId, opt => opt.MapFrom(src => src.TermId))
            .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectId));

        CreateMap<EnrollmentDto, Enrollment>()
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        CreateMap<Enrollment, CreateEnrollmentDto>().ReverseMap();
        CreateMap<Enrollment, UpdateEnrollmentDto>().ReverseMap();
    }
}
