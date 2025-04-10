using DogusTeknoloji_BlogApp.Services.DTOs.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Services.Interfaces
{
    public interface IUserService
    {
        bool CreateUser(CreateUserDto userDto);
        bool SignIn(SignInDto signIn);
        void SignOut();
        Task<UserProfileDto> GetUserProfileAsync(Guid userId);
    }
}
