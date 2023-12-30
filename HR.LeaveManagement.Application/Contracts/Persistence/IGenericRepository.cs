using HR.LeaveManagement.Domain.Common;

namespace HR.LeaveManagement.Application.Contracts.Persistence;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T> Create(T entity);
    Task<T> Update(T entity);
    Task<T> Delete(T entity);
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
}

