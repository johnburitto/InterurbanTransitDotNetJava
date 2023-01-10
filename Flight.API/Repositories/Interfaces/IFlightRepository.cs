using Flight.API.Dtos.Create;
using Flight.API.Dtos.Encrypting;
using Flight.API.Entities;

namespace Flight.API.Repositories.Interfaces
{
    public interface IFlightRepository : ICrudRepository<FlightEntity, FlightEntityCreateDto>
    {
        Task<IEnumerable<FlightEntityEncrypted>> GetFlightsEncryptedAsync();
        Task<FlightEntityEncrypted?> GetFlightByIdEncryptedAsync(int id);
    }
}
