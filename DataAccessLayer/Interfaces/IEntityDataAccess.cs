
namespace DataAccessLayer.Interfaces
{
    public interface IEntityDataAccess<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync<TKey>(TKey id);
        Task CreateAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}
