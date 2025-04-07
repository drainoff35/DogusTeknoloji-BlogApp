namespace DogusTeknoloji_BlogApp.Models.ViewModels.CommentViewModels
{
    public class CommentDetailViewModel
    {
        public string Text { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; } = null!;
    }
}
