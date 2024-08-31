using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface IUserService
{
    Task<bool> IsExistUserAsync(Guid userId);
    Task<UserDto?> GetUserByClerkId(string clerkId);
}