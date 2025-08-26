using AutoMapper;
using SchoolSync.Domain.Entities;
using SchoolSync.App.DTOs.Lesson;

namespace SchoolSync.App.MappingProfiles;

public class LessonProfile : Profile
{
    public LessonProfile()
    {
        CreateMap<Lesson, LessonDto>();
        CreateMap<CreateLessonDto, Lesson>();
        CreateMap<UpdateLessonDto, Lesson>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
