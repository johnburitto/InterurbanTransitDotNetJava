using Flight.API.Dtos.Create;
using Flight.API.Dtos.Encrypting;
using Flight.API.Entities;

namespace Flight.API.Repositories.Interfaces
{
    public interface IDriverRepository : ICrudRepository<Driver, DriverCreateDto>
    {
        Task<IEnumerable<DriverEncrypted>> GetDriversEncryptedAsync();
        Task<DriverEncrypted?> GetDriverByIdEncryptedAsync(int id);
    }
}
