using SchoolSync.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SchoolSync.Infra.Persistence;

namespace SchoolSync.Infra.Repositories;

public class GenericRepo<T>(DBContext context) : IGenericRepo<T> where T : class
{
    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        var combinedPredicate = CombineWithNotDeleted(predicate);
        return await dbSet.AnyAsync(combinedPredicate);
    }

    private Expression<Func<T, bool>> CombineWithNotDeleted(Expression<Func<T, bool>> predicate)
    {
        // Create IsDeleted == false expression
        var parameter = Expression.Parameter(typeof(T), "x");
        var isDeletedProperty = Expression.Property(parameter, "IsDeleted");
        var falseConstant = Expression.Constant(false);
        var notDeletedExpression = Expression.Equal(isDeletedProperty, falseConstant);

        // Replace parameter in original predicate
        var predicateBody = new ParameterReplacer(predicate.Parameters[0], parameter).Visit(predicate.Body);

        // Combine: predicate AND IsDeleted == false
        var combinedBody = Expression.AndAlso(predicateBody, notDeletedExpression);
        return Expression.Lambda<Func<T, bool>>(combinedBody, parameter);
    }

    private class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParameter;
        private readonly ParameterExpression _newParameter;

        public ParameterReplacer(ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            _oldParameter = oldParameter;
            _newParameter = newParameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _oldParameter ? _newParameter : base.VisitParameter(node);
        }
    }
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

    public async Task<T?> GetAsync(int id)
    {
        return await dbSet.Where(x => EF.Property<int>(x, "Id") == id && !EF.Property<bool>(x, "IsDeleted")).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await dbSet.Where(x => !EF.Property<bool>(x, "IsDeleted")).ToListAsync();
    }

    // Joker method for flexible querying
    public async Task<IEnumerable<T>> GetRangeWhereAsync(Expression<Func<T, bool>> predicate)
    {
        IQueryable<T> query = dbSet.Where(x => !EF.Property<bool>(x, "IsDeleted"));
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
        var entities = await dbSet.Where(x => !EF.Property<bool>(x, "IsDeleted")).Where(predicate).ToListAsync();

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

    // Soft delete logic - Set IsDeleted = true
    public async Task DeleteAsync(int id)
    {
        var entity = await dbSet.FindAsync(id);
        if (entity != null)
        {
            var prop = typeof(T).GetProperty("IsDeleted");
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(entity, true);
                dbSet.Update(entity);
            }
        }
    }

    public async Task DeleteRangeWhereAsync(Expression<Func<T, bool>> predicate)
    {
        var items = await dbSet.Where(x => !EF.Property<bool>(x, "IsDeleted")).Where(predicate).ToListAsync();
        var prop = typeof(T).GetProperty("IsDeleted");
        foreach (var item in items)
        {
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(item, true);
                dbSet.Update(item);
            }
        }
    }

    // Admin method - get including deleted records
    public async Task<IEnumerable<T>> GetAllIncludingDeletedAsync()
    {
        return await dbSet.ToListAsync();
    }

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}