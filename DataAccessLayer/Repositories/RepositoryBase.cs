using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    internal abstract class RepositoryBase<T>(DbContext context) : IEntityDataAccess<T> where T : class
    {
        protected readonly DbContext _context = context;

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync<T>();
        }

        public async Task<T> GetByIdAsync<TKey>(TKey id)
        {
            var entity =  await _context.Set<T>().FindAsync(id);
            if(entity is null)
            {
                throw new KeyNotFoundException($"Entity of type '{typeof(T).Name}' with ID {id} was not found.");
            }
            return entity;
        }

        public async Task CreateAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
            }

            _context.Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

    }
}
