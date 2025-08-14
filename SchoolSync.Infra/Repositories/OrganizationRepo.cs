using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Infra.Persistence;
namespace SchoolSync.Infra.Repositories;


// This is a wrapper for the Org Repo, because all its ops are generic
public class OrganizationRepo(DBContext context) : GenericRepo<Organization>(context), IOrganizationRepo { }
