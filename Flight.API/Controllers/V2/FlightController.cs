using Flight.API.Dtos.Encrypting;
using Flight.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flight.API.Controllers.V2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly IFlightRepository _repository;

        public FlightController(IFlightRepository repository)
        {
            _repository = repository;
        }

        #region API
        [Authorize(Roles = "User")]
        [HttpGet("get")]
        [ProducesResponseType(typeof(IEnumerable<FlightEntityEncrypted>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<FlightEntityEncrypted>>> GetAllFlights()
        {
            return Ok(await _repository.GetFlightsEncryptedAsync());
        }

        [Authorize(Roles = "User")]
        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(FlightEntityEncrypted), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FlightEntityEncrypted>> GetFlightById(int id)
        {
            var driver = await _repository.GetFlightByIdEncryptedAsync(id);

            return driver == null ? NotFound() : Ok(driver);
        }
        #endregion
    }
}
