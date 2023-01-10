using Flight.API.Dtos.Create;
using Flight.API.Dtos.Encrypting;
using Flight.API.Entities;

namespace Flight.API.Repositories.Interfaces
{
    public interface ITransportRepository : ICrudRepository<Transport, TransportCreateDto>
    {
        Task<IEnumerable<TransportEncrypted>> GetTransportsEncryptedAsync();
        Task<TransportEncrypted?> GetTransportByIdEncryptedAsync(int id);
    }
}
