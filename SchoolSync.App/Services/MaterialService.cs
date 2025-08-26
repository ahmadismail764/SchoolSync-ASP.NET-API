using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Domain.IServices;
namespace SchoolSync.App.Services;

public class MaterialService(
    IMaterialRepo materialRepo,
    ILessonRepo lessonRepo) : GenericService<Material>(materialRepo), IMaterialService
{
    private readonly ILessonRepo _lessonRepo = lessonRepo;

    public override async Task ValidateAsync(Material material)
    {
        ArgumentNullException.ThrowIfNull(material);

        if (string.IsNullOrWhiteSpace(material.FileName))
            throw new ArgumentException("FileName is required.", nameof(material.FileName));
        if (material.FileData == null || material.FileData.Length == 0)
            throw new ArgumentException("FileData cannot be empty.", nameof(material.FileData));
        if (material.LessonId <= 0)
            throw new ArgumentException("LessonId must be a positive integer.", nameof(material.LessonId));

        var lesson = await _lessonRepo.GetAsync(material.LessonId);
        if (lesson == null)
            throw new ArgumentException($"Lesson with ID {material.LessonId} does not exist.", nameof(material.LessonId));
    }

    public override async Task<Material> CreateAsync(Material material)
    {
        await ValidateAsync(material);
        var createdMaterial = await _repo.CreateAsync(material);
        await _repo.SaveChangesAsync();
        return createdMaterial;
    }

    public async Task<byte[]?> GetMaterialFileAsync(int materialId)
    {
        var material = await _repo.GetAsync(materialId);
        return material?.FileData;
    }
    public async Task<IEnumerable<Material>> GetByLessonIdAsync(int lessonId)
        => await _repo.GetRangeWhereAsync(m => m.LessonId == lessonId);
}
