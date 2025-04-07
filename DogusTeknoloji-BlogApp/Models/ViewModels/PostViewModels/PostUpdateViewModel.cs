namespace DogusTeknoloji_BlogApp.Models.ViewModels.PostViewModels
{
    public class PostUpdateViewModel
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int CategoryId { get; set; }
    }
}
