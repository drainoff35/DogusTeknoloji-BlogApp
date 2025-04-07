using DogusTeknoloji_BlogApp.Core.Entities;
using DogusTeknoloji_BlogApp.Models.ViewModels.PostViewModels;

namespace DogusTeknoloji_BlogApp.Models.ViewModels.CategoryViewModels
{
    public class CategoryDetailViewModel
    {
        public string Name { get; set; } = null!;
        public List<PostDetailViewModel> Posts { get; set; } = new List<PostDetailViewModel>();
    }
}
