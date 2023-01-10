using AutoMapper;
using Flight.API.Dtos.Create;
using Flight.API.Dtos.Update;
using Flight.API.Entities;
using Flight.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.API.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly IRouteRepository _repository;
        private readonly IMapper _mapper;

        public RouteController(IRouteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        #region CRUD
        [Authorize(Roles = "User")]
        [HttpGet("get")]
        [ProducesResponseType(typeof(IEnumerable<FlightRoute>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<FlightRoute>>> GetAllRoutes()
        {
            return Ok(await _repository.GetAllAsync());
        }

        [Authorize(Roles = "User")]
        [HttpGet("get/{id}", Name = "GetRouteById")]
        [ProducesResponseType(typeof(FlightRoute), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FlightRoute>> GetRouteById(int id)
        {
            var driver = await _repository.GetByIdAsync(id);

            return driver == null ? NotFound() : Ok(driver);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateRoute(FlightRouteCreateDto flightRouteCreateDto)
        {
            var route = await _repository.CreateAsync(flightRouteCreateDto);

            return CreatedAtRoute(nameof(GetRouteById), new { route.Id }, route);
        }

        [Authorize(Roles = "Operator")]
        [HttpPut("update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FlightRoute>> UpdateRoute(int id, FlightEntityUpdateDto flightEntityUpdateDto)
        {
            if (id != flightEntityUpdateDto.Id)
            {
                return BadRequest();
            };

            var route = _mapper.Map<FlightRoute>(flightEntityUpdateDto);
            var updatedRoute = await _repository.UpdateAsync(route);

            return Ok(updatedRoute);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteRoute(int id)
        {
            var routeToDelete = await _repository.GetByIdAsync(id);

            if (routeToDelete == null)
            {
                return NotFound();
            }

            await _repository.DeleteByIdAsync(routeToDelete);

            return NoContent();
        }
        #endregion
    }
}
