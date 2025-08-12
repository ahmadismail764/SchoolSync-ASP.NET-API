using System.Runtime.InteropServices;

namespace SchoolSync.Domain.IRepositories;

public interface IGenericRepo<T>
{
    T Get(int id);
    IEnumerable<T> GetAll();
    IEnumerable<T> GetRangeWhere(Func<T, bool> predicate);
    void Create(T entity);
    void Update(T entity);

    void UpdateRangeWhere(Func<T, bool> predicate, T entity);
    void Delete(T entity);
    void DeleteRangeWhere(Func<T, bool> predicate);
}
