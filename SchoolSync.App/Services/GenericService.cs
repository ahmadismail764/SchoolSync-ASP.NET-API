using SchoolSync.Domain.IServices;
using SchoolSync.Domain.IRepositories;
using System.Linq.Expressions;
namespace SchoolSync.App.Services;

public class GenericService<T>(IGenericRepo<T> repo) : IGenericService<T> where T : class
{
    private readonly IGenericRepo<T> _repo = repo;

    public async Task<T?> GetByIdAsync(int id) => await _repo.GetAsync(id);
    public async Task<IEnumerable<T>> GetAllAsync() => await _repo.GetAllAsync();
    public virtual async Task<T> CreateAsync(T entity)
    {
        await ValidateAsync(entity);
        var created = await _repo.CreateAsync(entity);
        await _repo.SaveChangesAsync();
        return created;
    }

    public virtual Task ValidateAsync(T entity)
    {
        return Task.CompletedTask;
    }

    public async Task UpdateAsync(T entity)
    {
        await _repo.UpdateAsync(entity);
        await _repo.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var entity = await _repo.GetAsync(id);
        if (entity != null)
        {
            await _repo.DeleteAsync(entity);
            await _repo.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<T>> GetRangeWhereAsync(Expression<Func<T, bool>> predicate)
        => await _repo.GetRangeWhereAsync(predicate);

    public async Task<IEnumerable<T>> UpdateRangeWhereAsync(Expression<Func<T, bool>> predicate, T entity)
    {
        await _repo.UpdateRangeWhereAsync(predicate, entity);
        await _repo.SaveChangesAsync();
        return await _repo.GetRangeWhereAsync(predicate);
    }

    public async Task<IEnumerable<T>> DeleteRangeWhereAsync(Expression<Func<T, bool>> predicate)
    {
        await _repo.DeleteRangeWhereAsync(predicate);
        await _repo.SaveChangesAsync();
        return await Task.FromResult(Enumerable.Empty<T>());
    }

    public async Task SaveChangesAsync()
    {
        await _repo.SaveChangesAsync();
    }
}
