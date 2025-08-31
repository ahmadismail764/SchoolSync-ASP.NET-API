using SchoolSync.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolSync.Domain.IRepositories;

public interface IMaterialRepo : IGenericRepo<Material>
{
    Task<IEnumerable<Material>> GetByLessonIdAsync(int lessonId);
    Task<Material?> GetByFileNameAsync(string FileName);
}
