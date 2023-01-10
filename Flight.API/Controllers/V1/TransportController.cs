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
    public class TransportController : ControllerBase
    {
        private readonly ITransportRepository _repository;
        private readonly IMapper _mapper;

        public TransportController(ITransportRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        #region CRUD
        [Authorize(Roles = "User")]
        [HttpGet("get")]
        [ProducesResponseType(typeof(IEnumerable<Transport>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Transport>>> GetAllTransports()
        {
            return Ok(await _repository.GetAllAsync());
        }

        [Authorize(Roles = "User")]
        [HttpGet("get/{id}", Name = "GetTransportById")]
        [ProducesResponseType(typeof(Transport), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Transport>> GetTransportById(int id)
        {
            var driver = await _repository.GetByIdAsync(id);

            return driver == null ? NotFound() : Ok(driver);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateTransport(TransportCreateDto transportCreateDto)
        {
            var transport = await _repository.CreateAsync(transportCreateDto);

            return CreatedAtRoute(nameof(GetTransportById), new { transport.Id }, transport);
        }

        [Authorize(Roles = "Operator")]
        [HttpPut("update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Transport>> UpdateTransport(int id, TransportUpdateDto transportUpdateDto)
        {
            if (id != transportUpdateDto.Id)
            {
                return BadRequest();
            };

            var transport = _mapper.Map<Transport>(transportUpdateDto);
            var updatedTransport = await _repository.UpdateAsync(transport);

            return Ok(updatedTransport);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteTransport(int id)
        {
            var transportToDelete = await _repository.GetByIdAsync(id);

            if (transportToDelete == null)
            {
                return NotFound();
            }

            await _repository.DeleteByIdAsync(transportToDelete);

            return NoContent();
        }
        #endregion
    }
}
