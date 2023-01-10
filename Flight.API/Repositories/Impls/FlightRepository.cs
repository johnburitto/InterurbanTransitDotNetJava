using AutoMapper;
using Flight.API.Data;
using Flight.API.Dtos.Create;
using Flight.API.Dtos.Encrypting;
using Flight.API.Entities;
using Flight.API.Repositories.Interfaces;
using Flight.API.Services.Encrypted;
using Microsoft.EntityFrameworkCore;

namespace Flight.API.Repositories.Impls
{
    public class FlightRepository : IFlightRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEncryptedService _encryptedService;

        public FlightRepository(AppDbContext context, IMapper mapper, IEncryptedService encryptedService)
        {
            _context = context;
            _mapper = mapper;
            _encryptedService = encryptedService;
        }

        public async Task<FlightEntity> CreateAsync(FlightEntityCreateDto entity)
        {
            var flight = _mapper.Map<FlightEntity>(entity);

            flight.CreatedAt = DateTime.UtcNow;
            flight.UpdatedAt = DateTime.UtcNow;

            await _context.Flights.AddAsync(flight);
            await _context.SaveChangesAsync();

            return flight;
        }

        public async Task DeleteByIdAsync(FlightEntity entity)
        {
            _context.Flights.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FlightEntity>> GetAllAsync()
        {
            return await _context.Flights
                .Include(flight => flight.Driver)
                .Include(flight => flight.Transport)
                .Include(flight => flight.Route)
                .ToListAsync();
        }

        public async Task<FlightEntity?> GetByIdAsync(int id)
        {
            return await _context.Flights
                .Include(flight => flight.Driver)
                .Include(flight => flight.Transport)
                .Include(flight => flight.Route)
                .FirstOrDefaultAsync(flight => flight.Id == id);
        }

        public async Task<FlightEntityEncrypted?> GetFlightByIdEncryptedAsync(int id)
        {
            var flight = await _context.Flights.FirstOrDefaultAsync(flight => flight.Id == id);

            return flight != null ? new FlightEntityEncrypted
            {
                Id = (await _encryptedService.Encrypt(flight.Id.ToString())).Data,
                DriverId = (await _encryptedService.Encrypt(flight.DriverId.ToString())).Data,
                TransportId = (await _encryptedService.Encrypt(flight.TransportId.ToString())).Data,
                RouteId = (await _encryptedService.Encrypt(flight.RouteId.ToString())).Data,
                StartDay = (await _encryptedService.Encrypt(flight.StartDay.ToString())).Data,
                EndDay = (await _encryptedService.Encrypt(flight.EndDay.ToString())).Data,
                CreatedAt = (await _encryptedService.Encrypt(flight.CreatedAt.ToString())).Data,
                UpdatedAt = (await _encryptedService.Encrypt(flight.UpdatedAt.ToString())).Data
            } : null;
        }

        public async Task<IEnumerable<FlightEntityEncrypted>> GetFlightsEncryptedAsync()
        {
            var flights = await _context.Flights.ToListAsync();
            var encryptedFlights = new List<FlightEntityEncrypted>();

            foreach (var flight in flights)
            {
                encryptedFlights.Add(new FlightEntityEncrypted
                {
                    Id = (await _encryptedService.Encrypt(flight.Id.ToString())).Data,
                    DriverId = (await _encryptedService.Encrypt(flight.DriverId.ToString())).Data,
                    TransportId = (await _encryptedService.Encrypt(flight.TransportId.ToString())).Data,
                    RouteId = (await _encryptedService.Encrypt(flight.RouteId.ToString())).Data,
                    StartDay = (await _encryptedService.Encrypt(flight.StartDay.ToString())).Data,
                    EndDay = (await _encryptedService.Encrypt(flight.EndDay.ToString())).Data,
                    CreatedAt = (await _encryptedService.Encrypt(flight.CreatedAt.ToString())).Data,
                    UpdatedAt = (await _encryptedService.Encrypt(flight.UpdatedAt.ToString())).Data
                });
            }

            return encryptedFlights;
        }

        public async Task<FlightEntity> UpdateAsync(FlightEntity entity)
        {
            entity.CreatedAt = _context.Flights.AsNoTracking().First(flight => flight.Id == entity.Id).CreatedAt;
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
