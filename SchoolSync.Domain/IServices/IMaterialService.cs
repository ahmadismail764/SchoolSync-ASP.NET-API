using SchoolSync.Domain.Entities;
using SchoolSync.Domain.Helpers;

namespace SchoolSync.Domain.IServices;

public interface IMaterialService : IGenericService<Material>
{
    Task<byte[]?> GetMaterialFileAsync(int materialId);
    Task<IEnumerable<Material>> GetByLessonIdAsync(int lessonId);
}
