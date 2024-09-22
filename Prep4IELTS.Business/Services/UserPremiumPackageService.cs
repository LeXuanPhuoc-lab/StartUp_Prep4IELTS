using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services;

public class UserPremiumPackageService(UnitOfWork unitOfWork) : IUserPremiumPackageService
{
    public async Task<bool> UpdateAsync(UserPremiumPackage userPremiumPackage)
    {
        var toUpdateEntity = 
            await unitOfWork.UserPremiumPackageRepository.FindOneWithConditionAsync(
                ump => ump.UserPremiumPackageId == userPremiumPackage.UserPremiumPackageId);

        if (toUpdateEntity == null) return false;
        
        // Update properties
        toUpdateEntity.ExpireDate = userPremiumPackage.ExpireDate;
        toUpdateEntity.IsActive = userPremiumPackage.IsActive;
        toUpdateEntity.PremiumPackageId = userPremiumPackage.PremiumPackageId;
        userPremiumPackage.UserId = userPremiumPackage.UserId;
        
        await unitOfWork.UserPremiumPackageRepository.UpdateAsync(toUpdateEntity);
        return await unitOfWork.UserPremiumPackageRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<UserPremiumPackageDto?> FindUserPremiumPackageAsync(Guid userId)
    {
        var userPremiumPackageEntity =
            await unitOfWork.UserPremiumPackageRepository.FindUserPremiumPackageAsync(userId);
        return userPremiumPackageEntity.Adapt<UserPremiumPackageDto>();
    }
}