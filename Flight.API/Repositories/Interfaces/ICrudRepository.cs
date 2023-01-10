using Flight.API.Dtos.Encrypting;

namespace Flight.API.Repositories.Interfaces
{
    public interface ICrudRepository<T, V> where T : class where V : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> CreateAsync(V entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteByIdAsync(T entity);
    }
}
