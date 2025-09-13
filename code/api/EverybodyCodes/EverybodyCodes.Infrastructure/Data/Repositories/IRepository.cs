using EverybodyCodes.Infrastructure.Data.Entities;

namespace EverybodyCodes.Infrastructure.Data.Repositories;

public interface IRepository<T> where T : EntityBase, new()
{
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
}
