namespace DogusTeknoloji_BlogApp.Models.ViewModels.UserViewModels
{
    public class CreateUserViewModel
    {
        public string UserName { get; set; } = null!;
        public DateTime RegisterDate { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;


    }
}
