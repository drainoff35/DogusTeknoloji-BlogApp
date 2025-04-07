namespace DogusTeknoloji_BlogApp.Models.ViewModels.PostViewModels
{
    public class PostDetailViewModel
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
    }
}
