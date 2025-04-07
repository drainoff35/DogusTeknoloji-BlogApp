using AutoMapper;
using DogusTeknoloji_BlogApp.Core.Entities;
using DogusTeknoloji_BlogApp.Models.ViewModels.PostViewModels;

namespace DogusTeknoloji_BlogApp.Mappings
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<PostCreateViewModel, Post>();
            CreateMap<PostUpdateViewModel, Post>();
            CreateMap<Post, PostUpdateViewModel>();
            CreateMap<Post, PostDetailViewModel>()
                .ForMember(vm => vm.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(vm => vm.UserName, opt => opt.MapFrom(src => src.User.UserName));
        }
    }
}
