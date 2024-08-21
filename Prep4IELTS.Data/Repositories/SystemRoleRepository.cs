using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Repositories;

public class SystemRoleRepository : GenericRepository<SystemRole>
{
    public SystemRoleRepository(Prep4IeltsContext dbContext) 
        : base(dbContext)
    {
    }
}