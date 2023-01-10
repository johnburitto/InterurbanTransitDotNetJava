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
    public class DriverRepository : IDriverRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEncryptedService _encryptedService;

        public DriverRepository(AppDbContext context, IMapper mapper, IEncryptedService encryptedService)
        {
            _context = context;
            _mapper = mapper;
            _encryptedService = encryptedService;
        }

        public async Task<Driver> CreateAsync(DriverCreateDto entity)
        {
            var driver = _mapper.Map<Driver>(entity);

            driver.CreatedAt = DateTime.UtcNow;
            driver.UpdatedAt = DateTime.UtcNow;

            await _context.Drivers.AddAsync(driver);
            await _context.SaveChangesAsync();

            return driver;
        }

        public async Task DeleteByIdAsync(Driver entity)
        {
            _context.Drivers.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Driver>> GetAllAsync()
        {
            return await _context.Drivers.Include(driver => driver.Flights)
                .ThenInclude(flight => flight.Transport)
                .Include(driver => driver.Flights)
                .ThenInclude(flight => flight.Route)
                .ToListAsync();
        }

        public async Task<Driver?> GetByIdAsync(int id)
        {
            return await _context.Drivers.Include(driver => driver.Flights)
                .ThenInclude(flight => flight.Transport)
                .Include(driver => driver.Flights)
                .ThenInclude(flight => flight.Route).FirstOrDefaultAsync(driver => driver.Id == id);
        }

        public async Task<DriverEncrypted?> GetDriverByIdEncryptedAsync(int id)
        {
            var driver = await _context.Drivers.FirstOrDefaultAsync(driver => driver.Id == id);

            return driver != null ? new DriverEncrypted
            {
                Id = (await _encryptedService.Encrypt(driver.Id.ToString())).Data,
                Name = (await _encryptedService.Encrypt(driver.Name)).Data,
                Experience = (await _encryptedService.Encrypt(driver.Experience.ToString())).Data,
                Category = (await _encryptedService.Encrypt(driver.Category.ToString())).Data,
                DateOfBirth = (await _encryptedService.Encrypt(driver.DateOfBirth.ToString())).Data,
                CreatedAt = (await _encryptedService.Encrypt(driver.CreatedAt.ToString())).Data,
                UpdatedAt = (await _encryptedService.Encrypt(driver.UpdatedAt.ToString())).Data
            } : null;
        }

        public async Task<IEnumerable<DriverEncrypted>> GetDriversEncryptedAsync()
        {
            var drivers = await _context.Drivers.ToListAsync();
            var encryptedDrivers = new List<DriverEncrypted>();

            foreach (var driver in drivers)
            {
                encryptedDrivers.Add(new DriverEncrypted
                {
                    Id = (await _encryptedService.Encrypt(driver.Id.ToString())).Data,
                    Name = (await _encryptedService.Encrypt(driver.Name)).Data,
                    Experience = (await _encryptedService.Encrypt(driver.Experience.ToString())).Data,
                    Category = (await _encryptedService.Encrypt(driver.Category.ToString())).Data,
                    DateOfBirth = (await _encryptedService.Encrypt(driver.DateOfBirth.ToString())).Data,
                    CreatedAt = (await _encryptedService.Encrypt(driver.CreatedAt.ToString())).Data,
                    UpdatedAt = (await _encryptedService.Encrypt(driver.UpdatedAt.ToString())).Data
                });
            }

            return encryptedDrivers;
        }

        public async Task<Driver> UpdateAsync(Driver entity)
        {
            entity.CreatedAt = _context.Drivers.AsNoTracking().First(driver => driver.Id == entity.Id).CreatedAt;
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
