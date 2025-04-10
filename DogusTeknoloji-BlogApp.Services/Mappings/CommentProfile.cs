using AutoMapper;
using DogusTeknoloji_BlogApp.Core.Entities;
using DogusTeknoloji_BlogApp.Services.DTOs.CommentDtos;

namespace DogusTeknoloji_BlogApp.Mappings
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDetailDto>()
                .ForMember(dto => dto.UserName, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<CommentCreateDto, Comment>();
            CreateMap<CommentUpdateDto, Comment>();
            CreateMap<Comment, CommentUpdateDto>();
        }
    }
}
