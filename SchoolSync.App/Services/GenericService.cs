using SchoolSync.Domain.IServices;
using SchoolSync.Domain.IRepositories;

namespace SchoolSync.App.Services;

public class GenericService<T>(IGenericRepo<T> repo) : IGenericService<T> where T : class
{
    private readonly IGenericRepo<T> _repo = repo;

    public async Task<T?> GetByIdAsync(int id) => await _repo.GetAsync(id);
    public async Task<IEnumerable<T>> GetAllAsync() => await _repo.GetAllAsync();
    public async Task CreateAsync(T entity) => await _repo.CreateAsync(entity);
    public async Task UpdateAsync(T entity) => await _repo.UpdateAsync(entity);
    public async Task DeleteAsync(int id)
    {
        var entity = await _repo.GetAsync(id);
        if (entity != null)
            await _repo.DeleteAsync(entity);
    }
}
