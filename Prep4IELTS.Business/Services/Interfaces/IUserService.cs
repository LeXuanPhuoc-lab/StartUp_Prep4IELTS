namespace Prep4IELTS.Business.Services.Interfaces;

public interface IUserService
{
    Task<bool> IsExistUserAsync(Guid userId);
}