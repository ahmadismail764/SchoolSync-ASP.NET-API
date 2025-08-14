using SchoolSync.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SchoolSync.Infra.Persistence;

namespace SchoolSync.Infra.Repositories;

public class GenericRepo<T>(DBContext context) : IGenericRepo<T> where T : class
{
    protected readonly DbContext context = context;
    protected readonly DbSet<T> dbSet = context.Set<T>();

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
        var prop = typeof(T).GetProperty("IsActive");
        if (prop != null && prop.CanWrite)
        {
            prop.SetValue(entity, false);
            dbSet.Update(entity);
        }
        // else do nothing (or throw if you want strict behavior)
        await Task.CompletedTask;
    }

    public async Task DeleteRangeWhereAsync(Expression<Func<T, bool>> predicate)
    {
        var items = await dbSet.Where(predicate).ToListAsync();
        var prop = typeof(T).GetProperty("IsActive");
        foreach (var item in items)
        {
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(item, false);
                dbSet.Update(item);
            }
        }
    }
}