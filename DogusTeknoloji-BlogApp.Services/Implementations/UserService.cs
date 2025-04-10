using AutoMapper;
    using DogusTeknoloji_BlogApp.Core.Entities;
    using DogusTeknoloji_BlogApp.Services.DTOs.UserDtos;
    using DogusTeknoloji_BlogApp.Services.Interfaces;
    using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace DogusTeknoloji_BlogApp.Services.Implementations
    {
        public class UserService : IUserService
        {
            private readonly RoleManager<AppRole> _roleManager;
            private readonly SignInManager<AppUser> _signInManager;
            private readonly UserManager<AppUser> _userManager;
            private readonly IMapper _mapper;

            public UserService(RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IMapper mapper)
            {
                _roleManager = roleManager;
                _signInManager = signInManager;
                _userManager = userManager;
                _mapper = mapper;
            }

            public bool CreateUser(CreateUserDto userDto)
            {
                try
                {
                    var user = _mapper.Map<AppUser>(userDto);
                    // Set NormalizedUserName and NormalizedEmail explicitly
                    user.NormalizedUserName = user.UserName?.ToUpper();
                    user.NormalizedEmail = user.Email?.ToUpper();

                    var result = _userManager.CreateAsync(user, userDto.Password).Result;

                    if (!result.Succeeded)
                    {
                        // Log the errors or save them for debugging
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        Console.WriteLine($"User creation failed: {errors}");
                    }

                    return result.Succeeded;
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Exception in CreateUser: {ex}");
                    return false;
                }
            }

        public async Task<UserProfileDto> GetUserProfileAsync(Guid userId)
        {
            var user = await _userManager.Users.Include(u => u.Posts).Include(u => u.Comments).FirstOrDefaultAsync(u => u.Id == userId);
            if(user == null)
            {
                throw new KeyNotFoundException("Kullanıcı bulunamadı.");
            }

            var userProfileDto = _mapper.Map<UserProfileDto>(user);
            return userProfileDto;
        }

        public bool SignIn(SignInDto signIn)
            {
                var existingUser = _userManager.FindByEmailAsync(signIn.Email).Result;
                if (existingUser == null)
                {
                    throw new KeyNotFoundException("Kullanıcı bulunamadı.");
                }
                var result = _signInManager.PasswordSignInAsync(existingUser.UserName!, signIn.Password, true, false).Result;
                return result.Succeeded;
            }

            public void SignOut()
            {
                _signInManager.SignOutAsync();
            }
        }
    }
