using System.Linq.Expressions;
namespace SchoolSync.Domain.IServices;

public interface IGenericService<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();

    Task<IEnumerable<T>> GetRangeWhereAsync(Expression<Func<T, bool>> predicate);
    Task<T> CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task<IEnumerable<T>> UpdateRangeWhereAsync(Expression<Func<T, bool>> predicate, T entity);
    Task DeleteAsync(int id);
    Task<IEnumerable<T>> DeleteRangeWhereAsync(Expression<Func<T, bool>> predicate);
    Task ValidateCreateAsync(T entity);
    Task ValidateUpdateAsync(T entity);
    Task SaveChangesAsync();
    Task<IEnumerable<T>> GetAllIncludingDeletedAsync(); // Admin method
}
