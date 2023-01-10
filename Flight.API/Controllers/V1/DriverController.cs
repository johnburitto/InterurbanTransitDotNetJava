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
    public class DriverController : ControllerBase
    {
        private readonly IDriverRepository _repository;
        private readonly IMapper _mapper;

        public DriverController(IDriverRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        #region CRUD
        [Authorize(Roles = "User")]
        [HttpGet("get")]
        [ProducesResponseType(typeof(IEnumerable<Driver>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Driver>>> GetAllDrivers()
        {
            return Ok(await _repository.GetAllAsync());
        }

        [Authorize(Roles = "User")]
        [HttpGet("get/{id}", Name = "GetDriverById")]
        [ProducesResponseType(typeof(Driver), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Driver>> GetDriverById(int id)
        {
            var driver = await _repository.GetByIdAsync(id);

            return driver == null ? NotFound() : Ok(driver);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateDriver(DriverCreateDto driverCreateDto)
        {
            var driver = await _repository.CreateAsync(driverCreateDto);

            return CreatedAtRoute(nameof(GetDriverById), new { driver.Id }, driver);
        }

        [Authorize(Roles = "Operator")]
        [HttpPut("update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Driver>> UpdateDriver(int id, DriverUpdateDto driverUpdateDto)
        {
            if (id != driverUpdateDto.Id)
            {
                return BadRequest();
            };

            var driver = _mapper.Map<Driver>(driverUpdateDto);
            var updatedDriver = await _repository.UpdateAsync(driver);

            return Ok(updatedDriver);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteDriver(int id)
        {
            var driverToDelete = await _repository.GetByIdAsync(id);

            if (driverToDelete == null)
            {
                return NotFound();
            }

            await _repository.DeleteByIdAsync(driverToDelete);

            return NoContent();
        }
        #endregion
    }
}
