using AutoMapper;
using DogusTeknoloji_BlogApp.Core.Entities;
using DogusTeknoloji_BlogApp.Services.DTOs.PostDtos;

namespace DogusTeknoloji_BlogApp.Mappings
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<PostCreateDto, Post>();
            CreateMap<PostUpdateDto, Post>();
            CreateMap<Post, PostUpdateDto>();
            CreateMap<Post, PostDetailDto>()
       .ForMember(dto => dto.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
       .ForMember(dto => dto.UserName, opt => opt.MapFrom(src => src.User.UserName))
       .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));
        }
    }
}
