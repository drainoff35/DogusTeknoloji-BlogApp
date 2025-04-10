using AutoMapper;
using DogusTeknoloji_BlogApp.Core.Entities;
using DogusTeknoloji_BlogApp.Services.DTOs.PostDtos;

namespace DogusTeknoloji_BlogApp.Mappings
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {

            CreateMap<Post, PostDetailDto>()
                .ForMember(dto => dto.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dto => dto.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dto => dto.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dto => dto.ImagePath, opt => opt.MapFrom(src => src.ImagePath))
                .ForMember(dto => dto.UserId, opt => opt.MapFrom(src => src.UserId));

            CreateMap<PostCreateDto, Post>()
                .ForMember(dto => dto.ImagePath, opt => opt.Ignore());

            CreateMap<PostUpdateDto, Post>()
                .ForMember(dto => dto.ImagePath, opt => opt.Ignore()); 

            CreateMap<Post, PostUpdateDto>()
                .ForMember(dto => dto.Image, opt => opt.Ignore())
                .ForMember(dto => dto.ExistingImagePath, opt => opt.MapFrom(src => src.ImagePath));
        }
    }
}

