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
    public class RouteController : ControllerBase
    {
        private readonly IRouteRepository _repository;

        public RouteController(IRouteRepository repository)
        {
            _repository = repository;
        }

        #region API
        [Authorize(Roles = "User")]
        [HttpGet("get")]
        [ProducesResponseType(typeof(IEnumerable<FlightRouteEncrypted>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<FlightRouteEncrypted>>> GetAllRoutes()
        {
            return Ok(await _repository.GetRoutesEncryptedAsync());
        }

        [Authorize(Roles = "User")]
        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(FlightRouteEncrypted), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FlightRouteEncrypted>> GetRouteById(int id)
        {
            var driver = await _repository.GetRouteByIdEncryptedAsync(id);

            return driver == null ? NotFound() : Ok(driver);
        }
        #endregion
    }
}
