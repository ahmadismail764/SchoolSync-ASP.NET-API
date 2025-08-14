using SchoolSync.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SchoolSync.Infra.Repositories;

public class GenericRepo<T> : IGenericRepo<T> where T : class
{
    protected readonly DbContext context;
    protected readonly DbSet<T> dbSet;

    public GenericRepo(DbContext context)
    {
        this.context = context;
        dbSet = context.Set<T>();
    }

    public async Task<T?> GetAsync(int id) => await dbSet.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync() => await dbSet.ToListAsync();

    public async Task<IEnumerable<T>> GetRangeWhereAsync(Expression<Func<T, bool>> predicate) =>
        await dbSet.Where(predicate).ToListAsync();

    public async Task CreateAsync(T entity) => await dbSet.AddAsync(entity);

    public async Task UpdateAsync(T entity)
    {
        dbSet.Update(entity);
        await Task.CompletedTask;
    }

    public async Task UpdateRangeWhereAsync(Expression<Func<T, bool>> predicate, T entity)
    {
        var items = await dbSet.Where(predicate).ToListAsync();
        foreach (var item in items)
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.CanWrite)
                {
                    var value = prop.GetValue(entity);
                    prop.SetValue(item, value);
                }
            }
            dbSet.Update(item);
        }
    }

    public async Task DeleteAsync(T entity)
    {
        dbSet.Remove(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeWhereAsync(Expression<Func<T, bool>> predicate)
    {
        var items = await dbSet.Where(predicate).ToListAsync();
        foreach (var item in items)
        {
            dbSet.Remove(item);
        }
    }
}