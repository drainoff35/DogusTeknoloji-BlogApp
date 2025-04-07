using AutoMapper;
using DogusTeknoloji_BlogApp.Core.Entities;
using DogusTeknoloji_BlogApp.Models.ViewModels.CommentViewModels;

namespace DogusTeknoloji_BlogApp.Mappings
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDetailViewModel>()
                .ForMember(vm => vm.UserName, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<CommentCreateViewModel, Comment>();
            CreateMap<CommentUpdateViewModel, Comment>();
            CreateMap<Comment, CommentUpdateViewModel>();
        }
    }
}
