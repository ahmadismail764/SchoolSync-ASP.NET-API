using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Infra.Persistence;

namespace SchoolSync.Infra.Repositories;

public class MaterialRepo(DBContext context) : GenericRepo<Material>(context), IMaterialRepo
{
}
