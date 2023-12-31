using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain.Common;
using HR.LeaveManagement.Persistence.DataBaseContexts;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly HRDatabaseContext _context;

        public GenericRepository(HRDatabaseContext context)
        {
            _context = context;
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> DeleteAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DoesEntityExistAsync(int id)
        {
            return await _context.Set<T>().AnyAsync(entity => entity.Id == id);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            var entity = await _context.Set<T>().AsNoTracking().ToListAsync();
            return entity;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _context.Set<T>().AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == id);
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Update(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
