using AutoMapper;
using DogusTeknoloji_BlogApp.Core.Entities;
using DogusTeknoloji_BlogApp.Models.ViewModels.CategoryViewModels;

namespace DogusTeknoloji_BlogApp.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryCreateViewModel, Category>();
            CreateMap<CategoryUpdateViewModel, Category>();
            CreateMap<Category, CategoryUpdateViewModel>();
            CreateMap<Category, CategoryDetailViewModel>();

        }
    }
}
