using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services;

public class UserService(UnitOfWork unitOfWork) : IUserService
{
    public async Task<bool> IsExistUserAsync(Guid userId)
    {
        return await unitOfWork.UserRepository.IsExistUserAsync(userId);
    }

    public async Task<UserDto?> GetUserByClerkId(string clerkId)
    {
        var user = await unitOfWork.UserRepository.GetUserByClerkId(clerkId);
        return user.Adapt<UserDto>();
    }
}