using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Security.Configurations;
using Security.Dtos;
using Security.Enteties;

namespace Security.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<string> RegisterAsync(UserRegistrationDto userRegistrationDto)
        {
            var newUser = _mapper.Map<AppUser>(userRegistrationDto);
            var userWithSameEmail = await _userManager.FindByEmailAsync(newUser.Email);

            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(newUser, userRegistrationDto.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, Authorization.DEFAULT_ROLE.ToString());
                }
                else
                {
                    return $"Some inyternal error has ocurred";
                }

                return $"User registered with username {newUser.UserName}";
            }
            else
            {
                return $"Email {newUser.Email} is already registered";
            }
        }
    }
}
