using System.Linq.Expressions;

namespace BookStoreAPI.Interfaces;

public interface IGenericRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression);
    Task<T?> GetByIdAsync(int id);
    Task<int> GetCountAsync();
    Task AddAsync(T entity);
    void Delete(T entity);
    void Update(T entity);
}