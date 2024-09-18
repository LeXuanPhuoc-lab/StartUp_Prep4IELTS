using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface IUserPremiumPackageService
{
    Task<bool> UpdateAsync(UserPremiumPackage userPremiumPackage);
    Task<UserPremiumPackageDto?> FindUserPremiumPackageAsync(Guid userId);
}