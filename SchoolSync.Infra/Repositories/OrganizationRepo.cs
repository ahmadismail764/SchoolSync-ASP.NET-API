using SchoolSync.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace SchoolSync.Infra.Repositories;

public class OrganizationRepo : GenericRepo<Organization>
{
    public OrganizationRepo(DbContext context) : base(context)
    {
    }
}
