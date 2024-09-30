using Mapster;
using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services;

public class UserPremiumPackageService(UnitOfWork unitOfWork) : IUserPremiumPackageService
{
    public async Task<bool> AddUserPremiumPackageTrialAsync(string clerkId)
    {
        return await unitOfWork.UserPremiumPackageRepository.AddUserPremiumPackageTrialAsync(clerkId);
    }

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

    public async Task UpdateUserPremiumPackageTrialAsync(Guid userId)
    {
        // var toUpdateEntity = 
        //     await unitOfWork.UserPremiumPackageRepository.FindOneWithConditionAsync(
        //         ump => ump.UserId == userId);
        //
        // if (toUpdateEntity == null) return;
        //
        // // Update user premium package
        // var currentTotalTrials = toUpdateEntity.TotalTrials;
        // toUpdateEntity.TotalTrials = currentTotalTrials > 1 
        //     ? currentTotalTrials - 1 
        //     : 0;
        //
        // // await unitOfWork.UserPremiumPackageRepository.UpdateAsync(toUpdateEntity);
        // await unitOfWork.UserPremiumPackageRepository.SaveChangeWithTransactionAsync();
        using (var dbContext = new Prep4IeltsContext())
        {
            var toUpdateEntity = 
                await dbContext.UserPremiumPackages.FirstOrDefaultAsync(
                    ump => ump.UserId == userId);
            
            if (toUpdateEntity == null) return;
            
            // Update user premium package
            var currentTotalTrials = toUpdateEntity.TotalTrials;
            toUpdateEntity.TotalTrials = currentTotalTrials > 1 
                ? currentTotalTrials - 1 
                : 0;

            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<UserPremiumPackageDto?> FindUserPremiumPackageAsync(Guid userId)
    {
        var userPremiumPackageEntity =
            await unitOfWork.UserPremiumPackageRepository.FindUserPremiumPackageAsync(userId);
        return userPremiumPackageEntity.Adapt<UserPremiumPackageDto>();
    }

    public async Task<UserPremiumPackageDto?> FindUserPremiumPackageAsync(Guid userId, string clerkId)
    {
        var userPremiumPackageEntity =
            await unitOfWork.UserPremiumPackageRepository.FindUserPremiumPackageAsync(userId, clerkId);
        return userPremiumPackageEntity.Adapt<UserPremiumPackageDto>();
    }
}