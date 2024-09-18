using System.Linq.Expressions;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface IPremiumPackageService
{
    Task<bool> InsertAsync(PremiumPackageDto premiumPackageDto);
    Task<bool> IsExistPremiumPackageAsync(int premiumPackageId);
    Task<bool> IsExistActiveUserPremiumPackageAsync(int premiumPackageId);
    Task<IList<PremiumPackageDto>> FindAllAsync();
    Task<IList<PremiumPackageDto>> FindAllDraftAsync();
    Task<bool> PublishAsync(int premiumPackageId);
    Task<bool> HideAsync(int premiumPackageId);
    Task<bool> IsAlreadyPublishedAsync(int premiumPackageId);
    Task UpdateAsync(PremiumPackageDto premiumPackageDto);
    Task<bool> RemoveAsync(int premiumPackageId);
    Task<bool> RemoveByDeActive(int premiumPackageId);
    Task<PremiumPackageDto> FindOneWithConditionAsync(
        Expression<Func<PremiumPackage, bool>> filter,
        Func<IQueryable<PremiumPackage>, IOrderedQueryable<PremiumPackage>>? orderBy = null, 
        string? includeProperties = "");
}