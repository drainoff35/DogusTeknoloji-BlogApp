using AutoMapper;
using DogusTeknoloji_BlogApp.Core.Entities;
using DogusTeknoloji_BlogApp.Services.DTOs.UserDtos;

namespace DogusTeknoloji_BlogApp.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, AppUser>();
            CreateMap<AppUser, CreateUserDto>();
            CreateMap<AppUser, UserProfileDto>()
                .ForMember(dest => dest.PostCount, opt => opt.MapFrom(src => src.Posts != null ? src.Posts.Count : 0))
                .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Comments != null ? src.Comments.Count : 0));
        }
    }
}
