using Security.Dtos;

namespace Security.Services
{
    public interface IUserService
    {
        Task<string> RegisterAsync(UserRegistrationDto userRegistrationDto);
    }
}
