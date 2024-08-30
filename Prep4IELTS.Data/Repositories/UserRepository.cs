using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Repositories;

public class UserRepository : GenericRepository<User>
{
    public UserRepository(Prep4IeltsContext dbContext) 
        : base(dbContext)
    {
    }

    public async Task<bool> IsExistUserAsync(Guid userId)
    {
        return await _dbSet.AnyAsync(x => x.UserId.Equals(userId));
    }
}