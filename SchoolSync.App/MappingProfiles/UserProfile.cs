using AutoMapper;
using SchoolSync.Domain.Entities;
using SchoolSync.App.DTOs.User;

namespace SchoolSync.App.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, CreateUserDto>().ReverseMap();
        CreateMap<User, UpdateUserDto>().ReverseMap();
    }
}
