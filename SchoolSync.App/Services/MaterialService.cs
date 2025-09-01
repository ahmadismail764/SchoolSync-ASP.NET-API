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

    public override Task ValidateCreateAsync(Material entity)
    {
        if (string.IsNullOrWhiteSpace(entity.FileName))
            throw new ArgumentException("File name is required.");
        if (string.IsNullOrWhiteSpace(entity.ContentType))
            throw new ArgumentException("Content type is required.");
        if (entity.FileData == null || entity.FileData.Length == 0)
            throw new ArgumentException("File data is required.");
        if (entity.FileSize <= 0)
            throw new ArgumentException("File size must be positive.");
        return Task.CompletedTask;
    }

    public override Task ValidateUpdateAsync(Material entity)
    {
        if (string.IsNullOrWhiteSpace(entity.FileName))
            throw new ArgumentException("File name is required.");
        if (string.IsNullOrWhiteSpace(entity.ContentType))
            throw new ArgumentException("Content type is required.");
        if (entity.FileData == null || entity.FileData.Length == 0)
            throw new ArgumentException("File data is required.");
        if (entity.FileSize <= 0)
            throw new ArgumentException("File size must be positive.");
        return Task.CompletedTask;
    }
}
