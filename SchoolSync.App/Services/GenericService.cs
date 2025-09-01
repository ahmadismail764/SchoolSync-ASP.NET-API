using SchoolSync.Domain.IServices;
using SchoolSync.Domain.IRepositories;
using System.Linq.Expressions;
namespace SchoolSync.App.Services;

public class GenericService<T>(IGenericRepo<T> repo) : IGenericService<T> where T : class
{
    protected readonly IGenericRepo<T> _repo = repo;


    // CRUD operations, mostly generic use cases
    public async Task<T?> GetByIdAsync(int id) => await _repo.GetAsync(id);
    public async Task<IEnumerable<T>> GetAllAsync() => await _repo.GetAllAsync();
    public async Task<IEnumerable<T>> GetRangeWhereAsync(Expression<Func<T, bool>> predicate)
    => await _repo.GetRangeWhereAsync(predicate);

    public virtual async Task<T> CreateAsync(T entity)
    {
        await ValidateCreateAsync(entity);
        var created = await _repo.CreateAsync(entity);
        await _repo.SaveChangesAsync();
        return created;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        await ValidateUpdateAsync(entity);
        await _repo.UpdateAsync(entity);
        await _repo.SaveChangesAsync();
    }
    public async Task<IEnumerable<T>> UpdateRangeWhereAsync(Expression<Func<T, bool>> predicate, T entity)
    {
        await _repo.UpdateRangeWhereAsync(predicate, entity);
        await _repo.SaveChangesAsync();
        return await _repo.GetRangeWhereAsync(predicate);
    }

    public virtual async Task DeleteAsync(int id)
    {
        await _repo.DeleteAsync(id);
        await _repo.SaveChangesAsync();
    }


    public async Task<IEnumerable<T>> DeleteRangeWhereAsync(Expression<Func<T, bool>> predicate)
    {
        await _repo.DeleteRangeWhereAsync(predicate);
        await _repo.SaveChangesAsync();
        return await Task.FromResult(Enumerable.Empty<T>());
    }


    // Utility functions start here
    public async Task SaveChangesAsync()
    {
        await _repo.SaveChangesAsync();
    }

    // Admin method - get including deleted records
    public async Task<IEnumerable<T>> GetAllIncludingDeletedAsync()
    {
        return await _repo.GetAllIncludingDeletedAsync();
    }

    // Validation function for use by different methods
    // To be overriden by every derived service
    public virtual Task ValidateCreateAsync(T entity)
    {
        return Task.CompletedTask;
    }

    public virtual Task ValidateUpdateAsync(T entity)
    {
        return Task.CompletedTask;
    }
    // Utility functions end here

}
