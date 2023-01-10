using AutoMapper;
using Security.Dtos;
using Security.Enteties;

namespace Security.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<UserRegistrationDto, AppUser>();
        }
    }
}
