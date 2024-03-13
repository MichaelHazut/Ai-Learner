using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public abstract class RepositoryBase<T>(DbContext context) : IEntityDataAccess<T> where T : class
    {
        public readonly DbContext _context = context;

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync<T>();
        }

        public async Task<T> GetByIdAsync<TKey>(TKey id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            return entity!;
        }

        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
            }

            _context.Entry(entity).State = EntityState.Modified;

        }

    }
}
