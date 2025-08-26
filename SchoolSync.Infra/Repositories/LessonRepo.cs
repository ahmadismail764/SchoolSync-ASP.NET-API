using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Infra.Persistence;

namespace SchoolSync.Infra.Repositories;

public class LessonRepo(DBContext context) : GenericRepo<Lesson>(context), ILessonRepo
{
    public async Task<IEnumerable<Lesson>> GetBySubjectIdAsync(int subjectId)
        => await GetRangeWhereAsync(l => l.SubjectId == subjectId);
}