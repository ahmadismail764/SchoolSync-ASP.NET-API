using System.Linq.Expressions;
namespace SchoolSync.Domain.IRepositories;

public interface IGenericRepo<T>
{
    Task<T?> GetAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetRangeWhereAsync(Expression<Func<T, bool>> predicate);
    Task<T> CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task UpdateRangeWhereAsync(Expression<Func<T, bool>> predicate, T entity);
    Task DeleteAsync(int id);
    Task DeleteRangeWhereAsync(Expression<Func<T, bool>> predicate);
    Task SaveChangesAsync();
}