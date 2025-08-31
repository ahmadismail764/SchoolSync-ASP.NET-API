using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;
namespace SchoolSync.App.Services;

public class MaterialService(
    IMaterialRepo materialRepo,
    ILessonRepo lessonRepo) : GenericService<Material>(materialRepo), IMaterialService
{
    private readonly ILessonRepo _lessonRepo = lessonRepo;


    public async Task<byte[]?> GetMaterialFileAsync(int materialId)
    {
        var material = await _repo.GetAsync(materialId);
        return material?.FileData;
    }
    public async Task<IEnumerable<Material>> GetByLessonIdAsync(int lessonId)
        => await _repo.GetRangeWhereAsync(m => m.LessonId == lessonId);
}
