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
    public class TransportRepository : ITransportRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEncryptedService _encryptedService;

        public TransportRepository(AppDbContext context, IMapper mapper, IEncryptedService encryptedService)
        {
            _context = context;
            _mapper = mapper;
            _encryptedService = encryptedService;
        }

        public async Task<Transport> CreateAsync(TransportCreateDto entity)
        {
            var transport = _mapper.Map<Transport>(entity);

            transport.CreatedAt = DateTime.UtcNow;
            transport.UpdatedAt = DateTime.UtcNow;

            await _context.Transports.AddAsync(transport);
            await _context.SaveChangesAsync();

            return transport;
        }

        public async Task DeleteByIdAsync(Transport entity)
        {
            _context.Transports.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Transport>> GetAllAsync()
        {
            return await _context.Transports.Include(transport => transport.Flights)
                .ThenInclude(flight => flight.Driver)
                .Include(transport => transport.Flights)
                .ThenInclude(flight => flight.Route)
                .ToListAsync();
        }

        public async Task<Transport?> GetByIdAsync(int id)
        {
            return await _context.Transports.Include(transport => transport.Flights)
                .ThenInclude(flight => flight.Driver)
                .Include(transport => transport.Flights)
                .ThenInclude(flight => flight.Route).FirstOrDefaultAsync(transport => transport.Id == id);
        }

        public async Task<TransportEncrypted?> GetTransportByIdEncryptedAsync(int id)
        {
            var transport = await _context.Transports.FirstOrDefaultAsync(transport => transport.Id == id);

            return transport != null ? new TransportEncrypted
            {
                Id = (await _encryptedService.Encrypt(transport.Id.ToString())).Data,
                Barnd = (await _encryptedService.Encrypt(transport.Barnd)).Data,
                Model = (await _encryptedService.Encrypt(transport.Model)).Data,
                Category = (await _encryptedService.Encrypt(transport.Category.ToString())).Data,
                ReleaseDate = (await _encryptedService.Encrypt(transport.ReleaseDate.ToString())).Data,
                CreatedAt = (await _encryptedService.Encrypt(transport.CreatedAt.ToString())).Data,
                UpdatedAt = (await _encryptedService.Encrypt(transport.UpdatedAt.ToString())).Data
            } : null;
        }

        public async Task<IEnumerable<TransportEncrypted>> GetTransportsEncryptedAsync()
        {
            var transports = await _context.Transports.ToListAsync();
            var encrypyedTransports = new List<TransportEncrypted>();

            foreach (var transport in transports)
            {
                encrypyedTransports.Add(new TransportEncrypted
                {
                    Id = (await _encryptedService.Encrypt(transport.Id.ToString())).Data,
                    Barnd = (await _encryptedService.Encrypt(transport.Barnd)).Data,
                    Model = (await _encryptedService.Encrypt(transport.Model)).Data,
                    Category = (await _encryptedService.Encrypt(transport.Category.ToString())).Data,
                    ReleaseDate = (await _encryptedService.Encrypt(transport.ReleaseDate.ToString())).Data,
                    CreatedAt = (await _encryptedService.Encrypt(transport.CreatedAt.ToString())).Data,
                    UpdatedAt = (await _encryptedService.Encrypt(transport.UpdatedAt.ToString())).Data
                });
            }

            return encrypyedTransports;
        }

        public async Task<Transport> UpdateAsync(Transport entity)
        {
            entity.CreatedAt = _context.Transports.AsNoTracking().First(transport => transport.Id == entity.Id).CreatedAt;
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
