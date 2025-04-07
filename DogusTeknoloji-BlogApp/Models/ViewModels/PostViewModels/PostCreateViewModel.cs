namespace DogusTeknoloji_BlogApp.Models.ViewModels.PostViewModels
{
    public class PostCreateViewModel
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int CategoryId { get; set; }
    }
}
