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
    Task<bool> IsExistUsername(string username);
    Task<int> CountTotalAsync();
    Task<int> CountTotalActiveAsync();
    Task<int> CountTotalInActiveAsync();
    Task<UserDto?> FindOneWithConditionAsync(
        Expression<Func<User, bool>> filter,
        Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, 
        string? includeProperties = "");
    Task<List<UserDto>> FindAllWithConditionAsync(
        Expression<Func<User, bool>> filter,
        Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, 
        string? includeProperties = "");
    Task<UserDto?> GetUserByClerkId(string clerkId);
    Task<UserDto?> FindByEmailAsync(string email);
    Task<IList<UserDto>> FindAllAsync();
    Task<IList<UserDto>> FindAllInActiveAsync();
    Task<IList<UserDto>> FindAllWithConditionAndPagingAsync(
        Expression<Func<User, bool>>? filter,
        Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy,
        string? includeProperties,
        int? pageIndex, int? pageSize);
}