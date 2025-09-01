using AutoMapper;
using SchoolSync.Domain.Entities;
using SchoolSync.App.DTOs.User;

namespace SchoolSync.App.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.SchoolId, opt => opt.MapFrom(src => src.SchoolId))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId));

        CreateMap<UserDto, User>()
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        CreateMap<User, CreateUserDto>().ReverseMap();
        CreateMap<User, UpdateUserDto>().ReverseMap();
    }
}
