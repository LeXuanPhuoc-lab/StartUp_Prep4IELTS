using System.Linq.Expressions;
using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services;

public class PremiumPackageService(UnitOfWork unitOfWork) : IPremiumPackageService
{
    public async Task<bool> InsertAsync(PremiumPackageDto premiumPackageDto)
    {
        var premiumPackage = premiumPackageDto.Adapt<PremiumPackage>();
        
        // Create datetime
        premiumPackage.CreateDate = DateTime.UtcNow;

        // Default create new premium package is inactive 
        premiumPackage.IsActive = false;
        
        // Progress create new package
        await unitOfWork.PremiumPackageRepository.InsertAsync(premiumPackage);
        return await unitOfWork.PremiumPackageRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<bool> IsExistPremiumPackageAsync(int premiumPackageId)
    {
        return await unitOfWork.PremiumPackageRepository.IsExistPremiumPackageAsync(premiumPackageId);
    }

    public async Task<bool> IsExistActiveUserPremiumPackageAsync(int premiumPackageId)
    {
        return await unitOfWork.PremiumPackageRepository.IsExistActiveUserPremiumPackageAsync(premiumPackageId);
    }

    public async Task<IList<PremiumPackageDto>> FindAllAsync()
    {
        var premiumPackageEntities = await unitOfWork.PremiumPackageRepository.FindAllAsync();
        return premiumPackageEntities.Adapt<List<PremiumPackageDto>>();
    }

    public async Task<IList<PremiumPackageDto>> FindAllDraftAsync()
    {
        var premiumPackageDtos = await unitOfWork.PremiumPackageRepository.FindAllDraftAsync();
        return premiumPackageDtos.Adapt<List<PremiumPackageDto>>();
    }

    public async Task<bool> PublishAsync(int premiumPackageId)
    {
        return await unitOfWork.PremiumPackageRepository.PublishAsync(premiumPackageId);
    }

    public async Task<bool> HideAsync(int premiumPackageId)
    {
        return await unitOfWork.PremiumPackageRepository.HideAsync(premiumPackageId);
    }

    public async Task<bool> IsAlreadyPublishedAsync(int premiumPackageId)
    {
        return await unitOfWork.PremiumPackageRepository.IsAlreadyPublishedAsync(premiumPackageId);
    }

    public async Task UpdateAsync(PremiumPackageDto premiumPackageDto)
    {
        // Get premium package by id 
        var premiumPackageEntity = await unitOfWork.PremiumPackageRepository.FindOneWithConditionAsync(
            pp => pp.PremiumPackageId == premiumPackageDto.PremiumPackageId);

        if (premiumPackageEntity == null) return;
        
        // Update properties 
        premiumPackageEntity.PremiumPackageName = premiumPackageDto.PremiumPackageName;
        premiumPackageEntity.DurationInMonths = premiumPackageDto.DurationInMonths;
        premiumPackageEntity.Description = premiumPackageDto.Description; 
        premiumPackageEntity.Price = premiumPackageDto.Price;

        await unitOfWork.PremiumPackageRepository.SaveChangeWithTransactionAsync();
    }

    public async Task<bool> RemoveAsync(int premiumPackageId)
    {
        await unitOfWork.PremiumPackageRepository.RemoveAsync(premiumPackageId);
        return await unitOfWork.PremiumPackageRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<bool> RemoveByDeActive(int premiumPackageId)
    {
        await unitOfWork.PremiumPackageRepository.RemoveByDeActive(premiumPackageId);
        return await unitOfWork.PremiumPackageRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<PremiumPackageDto> FindOneWithConditionAsync(Expression<Func<PremiumPackage, bool>> filter, Func<IQueryable<PremiumPackage>, IOrderedQueryable<PremiumPackage>>? orderBy = null, string? includeProperties = "")
    {
        var packageEntity = await unitOfWork.PremiumPackageRepository.FindOneWithConditionAsync(filter, orderBy, includeProperties);
        return packageEntity.Adapt<PremiumPackageDto>();
    }
}