using System.Linq.Expressions;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface IUserService
{
    Task<bool> InsertAsync(UserDto userDto);
    Task<bool> UpdateAsync(UserDto userDto, bool? isUpdateClerkId, bool? isUpdateRole);
    Task<bool> RemoveAsync(Guid userId, bool forceDelete);
    Task<bool> IsExistUserAsync(Guid userId);
    Task<UserDto?> FindOneWithConditionAsync(
        Expression<Func<User, bool>> filter,
        Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, 
        string? includeProperties = "");
    Task<UserDto?> GetUserByClerkId(string clerkId);
    Task<UserDto?> FindByEmailAsync(string email);
}