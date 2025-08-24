using SchoolSync.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SchoolSync.Infra.Persistence;

namespace SchoolSync.Infra.Repositories;

public class GenericRepo<T>(DBContext context) : IGenericRepo<T> where T : class
{
    protected readonly DbContext context = context;
    protected readonly DbSet<T> dbSet = context.Set<T>();

    protected virtual List<string> GetIncludes()
    {
        var navProps = typeof(T).GetProperties()
            .Where(p =>
                (p.PropertyType.IsClass && p.PropertyType != typeof(string)) ||
                (p.PropertyType.IsGenericType && typeof(System.Collections.IEnumerable).IsAssignableFrom(p.PropertyType) && p.PropertyType != typeof(string))
            )
            .Select(p => p.Name)
            .ToList();
        return navProps;
    }

    public async Task<T?> GetAsync(int id) => await dbSet.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync() => await dbSet.ToListAsync();

    // Joker method for flexible querying
    public async Task<IEnumerable<T>> GetRangeWhereAsync(Expression<Func<T, bool>> predicate)
    {
        IQueryable<T> query = dbSet;
        foreach (var include in GetIncludes())
        {
            query = query.Include(include);
        }
        return await query.Where(predicate).ToListAsync();
    }

    public async Task<T> CreateAsync(T entity)
    {
        await dbSet.AddAsync(entity);
        return entity;
    }


    public async Task UpdateAsync(T entity)
    {
        dbSet.Update(entity);
        await Task.CompletedTask;
    }

    public async Task UpdateRangeWhereAsync(Expression<Func<T, bool>> predicate, T updatedEntity)
    {
        var entities = await dbSet.Where(predicate).ToListAsync();

        var properties = typeof(T).GetProperties()
            .Where(p => p.CanWrite && p.GetMethod != null && !p.GetMethod.IsVirtual && p.Name != "Id"); // Exclude PK and navigation

        foreach (var entity in entities)
        {
            foreach (var prop in properties)
            {
                var newValue = prop.GetValue(updatedEntity);
                if (newValue != null)
                    prop.SetValue(entity, newValue);
            }
        }
    }

    // Soft delete logic, data-analysis-friendly
    public async Task DeleteAsync(int id)
    {
        var entity = await dbSet.FindAsync(id);
        if (entity != null)
        {
            var prop = typeof(T).GetProperty("IsActive");
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(entity, false);
                dbSet.Update(entity);
            }
        }
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

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}