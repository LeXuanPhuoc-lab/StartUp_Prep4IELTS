using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Repositories;

public class PremiumPackageRepository : GenericRepository<PremiumPackage>
{
    public PremiumPackageRepository(Prep4IeltsContext dbContext) 
        : base(dbContext)
    {
    }

    public async Task<List<PremiumPackage>> FindAllDraftAsync()
    {
        return await _dbSet.Where(pp => !pp.IsActive).ToListAsync();
    }

    public async Task<bool> IsExistPremiumPackageAsync(int premiumPackageId)
    {
        return await _dbSet.AnyAsync(pp => pp.PremiumPackageId == premiumPackageId);
    }

    public async Task<bool> IsAlreadyPublishedAsync(int premiumPackageId)
    {
        return await _dbSet.AnyAsync(pp => pp.PremiumPackageId == premiumPackageId && pp.IsActive);
    }

    public async Task<bool> IsExistActiveUserPremiumPackageAsync(int premiumPackageId)
    {
        var premiumPackages = await _dbSet
            .Include(pp => pp.UserPremiumPackages.Where(uup => uup.IsActive))
            .Where(pp => pp.PremiumPackageId == premiumPackageId)
            .ToListAsync();
        
        return premiumPackages.SelectMany(pp => pp.UserPremiumPackages).Any();
    }
    
    public async Task<bool> PublishAsync(int premiumPackageId)
    {
        var premiumPackageEntity = await _dbSet.FirstOrDefaultAsync(pp => pp.PremiumPackageId == premiumPackageId);
        if (premiumPackageEntity == null) return false;
        
        premiumPackageEntity.IsActive = true;

        return await SaveChangeWithTransactionAsync() > 0;
    }
    
    public async Task<bool> HideAsync(int premiumPackageId)
    {
        var premiumPackageEntity = await _dbSet.FirstOrDefaultAsync(pp => pp.PremiumPackageId == premiumPackageId);
        if (premiumPackageEntity == null) return false;
        
        premiumPackageEntity.IsActive = false;
        
        return await SaveChangeWithTransactionAsync() > 0;
    }

    public async Task RemoveByDeActive(int premiumPackageId)
    {
        var entityToDelete = await _dbSet.FindAsync(premiumPackageId);
        
        if (entityToDelete == null) throw new NullReferenceException(nameof(entityToDelete) + "is not found");
        
        // Set to de-active status
        entityToDelete.IsActive = false;
    }
    
    public override async Task RemoveAsync(object id)
    {
        var entityToDelete = await _dbSet
            .AsSplitQuery()
            .Include(pp => pp.UserPremiumPackages)
            .ThenInclude(upp => upp.Transactions)
            .FirstOrDefaultAsync(x => x.PremiumPackageId == (int) id);

        if (entityToDelete == null) throw new NullReferenceException(nameof(entityToDelete) + "is not found");
        
        if (DbContext.Entry(entityToDelete).State == EntityState.Detached)
        {
            _dbSet.Attach(entityToDelete);
        }
        _dbSet.Remove(entityToDelete);
    }
}