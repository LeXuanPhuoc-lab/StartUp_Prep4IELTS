using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Constants;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;
using Prep4IELTS.Data.Enum;
using Prep4IELTS.Data.Extensions;

namespace Prep4IELTS.Data.Repositories;

public class UserPremiumPackageRepository : GenericRepository<UserPremiumPackage>
{
    public UserPremiumPackageRepository(Prep4IeltsContext dbContext) 
        : base(dbContext)
    {
    }

    public async Task<bool> AddUserPremiumPackageTrialAsync(string clerkId)
    {
        // Find user by clerk id 
        var user = await DbContext.Users.FirstOrDefaultAsync(u => u.ClerkId == clerkId);
        // Get all premium package
        var standardPremiumPackage = await DbContext
            .PremiumPackages
            .FirstOrDefaultAsync(x => x.PackageType.Equals(
                PremiumPackageType.Standard.GetDescription()));
        
        // Check exist 
        if (standardPremiumPackage == null || user == null) return false;
        
        // Add user with the highest premium level for trials purpose
        await _dbSet.AddAsync(new ()
        {
            UserId = user.UserId,
            PremiumPackageId = standardPremiumPackage.PremiumPackageId,
            IsActive = false,
            TotalTrials = UserPremiumPackageConstants.TotalPremiumTrials,
            ExpireDate = DateTime.UtcNow.AddDays(UserPremiumPackageConstants.TrialsExpirationTotalDay)
        });
        return await SaveChangeWithTransactionAsync() > 0;
    }
    
    public async Task<UserPremiumPackage?> FindUserPremiumPackageAsync(Guid userId)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(pp => pp.PremiumPackage)
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }
    
    public async Task<UserPremiumPackage?> FindUserPremiumPackageAsync(Guid userId, string clerkId)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(pp => pp.PremiumPackage)
            .Include(pp => pp.User)
            .FirstOrDefaultAsync(x => x.User.ClerkId == clerkId && x.UserId == userId);
    }
}