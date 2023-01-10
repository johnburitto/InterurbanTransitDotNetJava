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
    public class FlightController : ControllerBase
    {
        private readonly IFlightRepository _repository;
        private readonly IMapper _mapper;

        public FlightController(IFlightRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        #region CRUD
        [Authorize(Roles = "User")]
        [HttpGet("get")]
        [ProducesResponseType(typeof(IEnumerable<FlightEntity>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<FlightEntity>>> GetAllFlights()
        {
            return Ok(await _repository.GetAllAsync());
        }

        [Authorize(Roles = "User")]
        [HttpGet("get/{id}", Name = "GetFlightById")]
        [ProducesResponseType(typeof(FlightEntity), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FlightEntity>> GetFlightById(int id)
        {
            var flight = await _repository.GetByIdAsync(id);

            return flight == null ? NotFound() : Ok(flight);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateFlight(FlightEntityCreateDto flightEntityCreateDto)
        {
            var flight = await _repository.CreateAsync(flightEntityCreateDto);

            return CreatedAtRoute(nameof(GetFlightById), new { flight.Id }, flight);
        }

        [Authorize(Roles = "Operator")]
        [HttpPut("update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FlightEntity>> UpdateFlight(int id, FlightEntityUpdateDto flightEntityUpdateDto)
        {
            if (id != flightEntityUpdateDto.Id)
            {
                return BadRequest();
            };

            var flight = _mapper.Map<FlightEntity>(flightEntityUpdateDto);
            var updatedFlight = await _repository.UpdateAsync(flight);

            return Ok(updatedFlight);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteFlight(int id)
        {
            var flightToDelete = await _repository.GetByIdAsync(id);

            if (flightToDelete == null)
            {
                return NotFound();
            }

            await _repository.DeleteByIdAsync(flightToDelete);

            return NoContent();
        }
        #endregion
    }
}
