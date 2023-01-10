using Flight.API.Dtos.Encrypting;
using Flight.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.API.Controllers.V2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverRepository _repository;

        public DriverController(IDriverRepository repository)
        {
            _repository = repository;
        }

        #region API
        [Authorize(Roles = "User")]
        [HttpGet("get")]
        [ProducesResponseType(typeof(IEnumerable<DriverEncrypted>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DriverEncrypted>>> GetAllDrivers()
        {
            return Ok(await _repository.GetDriversEncryptedAsync());
        }

        [Authorize(Roles = "User")]
        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(DriverEncrypted), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DriverEncrypted>> GetDriverById(int id)
        {
            var driver = await _repository.GetDriverByIdEncryptedAsync(id);

            return driver == null ? NotFound() : Ok(driver);
        }
        #endregion
    }
}
