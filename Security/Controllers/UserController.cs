using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Security.Dtos;
using Security.Services;

namespace Security.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #region API
        [HttpPost("register")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Register(UserRegistrationDto userRegistrationDto)
        {
            return Ok(await _userService.RegisterAsync(userRegistrationDto));
        }
        #endregion
    }
}
