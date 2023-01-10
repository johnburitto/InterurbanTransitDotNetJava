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
    public class RouteRepository : IRouteRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEncryptedService _encryptedService;

        public RouteRepository(AppDbContext context, IMapper mapper, IEncryptedService encryptedService)
        {
            _context = context;
            _mapper = mapper;
            _encryptedService = encryptedService;
        }

        public async Task<FlightRoute> CreateAsync(FlightRouteCreateDto entity)
        {
            var route = _mapper.Map<FlightRoute>(entity);

            route.CreatedAt = DateTime.UtcNow;
            route.UpdatedAt = DateTime.UtcNow;

            await _context.Routes.AddAsync(route);
            await _context.SaveChangesAsync();

            return route;
        }

        public async Task DeleteByIdAsync(FlightRoute entity)
        {
            _context.Routes.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FlightRoute>> GetAllAsync()
        {
            return await _context.Routes.Include(route => route.Flights)
                .ThenInclude(flight => flight.Driver)
                .Include(route => route.Flights)
                .ThenInclude(flight => flight.Transport)
                .ToListAsync();
        }

        public async Task<FlightRoute?> GetByIdAsync(int id)
        {
            return await _context.Routes.Include(route => route.Flights)
                .ThenInclude(flight => flight.Driver)
                .Include(route => route.Flights)
                .ThenInclude(flight => flight.Transport).FirstOrDefaultAsync(route => route.Id == id);
        }

        public async Task<FlightRouteEncrypted?> GetRouteByIdEncryptedAsync(int id)
        {
            var route = await _context.Routes.FirstOrDefaultAsync(route => route.Id == id);

            return route != null ? new FlightRouteEncrypted
            {
                Id = (await _encryptedService.Encrypt(route.Id.ToString())).Data,
                FromCity = (await _encryptedService.Encrypt(route.FromCity)).Data,
                ToCity = (await _encryptedService.Encrypt(route.ToCity)).Data,
                DepartureTime = (await _encryptedService.Encrypt(route.DepartureTime.ToString())).Data,
                ArrivalTime = (await _encryptedService.Encrypt(route.ArrivalTime.ToString())).Data,
                CreatedAt = (await _encryptedService.Encrypt(route.CreatedAt.ToString())).Data,
                UpdatedAt = (await _encryptedService.Encrypt(route.UpdatedAt.ToString())).Data
            } : null;
        }

        public async Task<IEnumerable<FlightRouteEncrypted>> GetRoutesEncryptedAsync()
        {
            var routes = await _context.Routes.ToListAsync();
            var encryptedRoutes = new List<FlightRouteEncrypted>();

            foreach (var route in routes)
            {
                encryptedRoutes.Add(new FlightRouteEncrypted
                {
                    Id = (await _encryptedService.Encrypt(route.Id.ToString())).Data,
                    FromCity = (await _encryptedService.Encrypt(route.FromCity)).Data,
                    ToCity = (await _encryptedService.Encrypt(route.ToCity)).Data,
                    DepartureTime = (await _encryptedService.Encrypt(route.DepartureTime.ToString())).Data,
                    ArrivalTime = (await _encryptedService.Encrypt(route.ArrivalTime.ToString())).Data,
                    CreatedAt = (await _encryptedService.Encrypt(route.CreatedAt.ToString())).Data,
                    UpdatedAt = (await _encryptedService.Encrypt(route.UpdatedAt.ToString())).Data
                });
            }

            return encryptedRoutes;
        }

        public async Task<FlightRoute> UpdateAsync(FlightRoute entity)
        {
            entity.CreatedAt = _context.Routes.AsNoTracking().First(route => route.Id == entity.Id).CreatedAt;
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
