using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;

namespace Prep4IELTS.Business.Services;

public class UserService(UnitOfWork unitOfWork) : IUserService
{
    public async Task<bool> IsExistUserAsync(Guid userId)
    {
        return await unitOfWork.UserRepository.IsExistUserAsync(userId);
    }
}