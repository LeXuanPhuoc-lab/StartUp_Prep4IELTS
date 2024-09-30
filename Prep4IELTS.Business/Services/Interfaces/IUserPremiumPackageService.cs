using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface IUserPremiumPackageService
{
    Task<bool> AddUserPremiumPackageTrialAsync(string clerkId);
    Task<bool> UpdateAsync(UserPremiumPackage userPremiumPackage);
    Task UpdateUserPremiumPackageTrialAsync(Guid userId);
    Task<UserPremiumPackageDto?> FindUserPremiumPackageAsync(Guid userId);
    Task<UserPremiumPackageDto?> FindUserPremiumPackageAsync(Guid userId, string clerkId);
}