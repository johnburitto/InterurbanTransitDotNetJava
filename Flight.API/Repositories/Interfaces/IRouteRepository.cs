using Flight.API.Dtos.Create;
using Flight.API.Dtos.Encrypting;
using Flight.API.Entities;

namespace Flight.API.Repositories.Interfaces
{
    public interface IRouteRepository : ICrudRepository<FlightRoute, FlightRouteCreateDto>
    {
        Task<IEnumerable<FlightRouteEncrypted>> GetRoutesEncryptedAsync();
        Task<FlightRouteEncrypted?> GetRouteByIdEncryptedAsync(int id);
    }
}
