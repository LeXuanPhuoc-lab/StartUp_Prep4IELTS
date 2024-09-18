using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Repositories;

public class UserPremiumPackageRepository : GenericRepository<UserPremiumPackage>
{
    public UserPremiumPackageRepository(Prep4IeltsContext dbContext) 
        : base(dbContext)
    {
    }

    public Task<UserPremiumPackage?> FindUserPremiumPackageAsync(Guid userId)
    {
        return _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);
    }
}