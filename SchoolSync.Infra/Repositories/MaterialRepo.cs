using Microsoft.EntityFrameworkCore;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Infra.Persistence;
namespace SchoolSync.Infra.Repositories;

public class MaterialRepo(DBContext context) : GenericRepo<Material>(context), IMaterialRepo
{
    public async Task<IEnumerable<Material>> GetByLessonIdAsync(int lessonId)
        => await dbSet.Where(m => m.LessonId == lessonId).OrderByDescending(m => m.UploadDate).ToListAsync();

    public async Task<Material?> GetByFileNameAsync(string FileName)
        => await dbSet.FirstOrDefaultAsync(m => m.FileName == FileName);
}
