using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;
using Prep4IELTS.Data.Extensions;

namespace Prep4IELTS.Data.Repositories;

public class SystemRoleRepository : GenericRepository<SystemRole>
{
    public SystemRoleRepository(Prep4IeltsContext dbContext) 
        : base(dbContext)
    {
    }

    public async Task<SystemRole?> FindByRoleNameAsync(Enum.SystemRole systemRole)
    {
        return await _dbSet.FirstOrDefaultAsync(s => s.RoleName == systemRole.GetDescription());
    }

    public async Task<bool> IsExistRoleAsync(int roleId)
    {
        return await _dbSet.AnyAsync(s => s.RoleId == roleId);
    }
}