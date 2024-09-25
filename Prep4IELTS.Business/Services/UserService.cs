using System.Linq.Expressions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services;

public class UserService(
    UnitOfWork unitOfWork,
    ICommentService commentService,
    ITestService testService,
    ITestHistoryService testHistoryService) : IUserService
{
    public async Task<bool> InsertAsync(UserDto userDto)
    {
        await unitOfWork.UserRepository.InsertAsync(userDto.Adapt<User>());
        return await unitOfWork.UserRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<bool> UpdateAsync(UserDto userDto, 
        bool? isUpdateClerkId = false,
        bool? isUpdateRole = false)
    {
        var userEntity = await unitOfWork.UserRepository.FindByEmailAsync(userDto.Email);

        if (userEntity == null) return false;
        
        // Check for update clerkId permission
        if(isUpdateClerkId.HasValue && isUpdateClerkId.Value) 
            userEntity.ClerkId = userDto.ClerkId;
        
        // Check for update role permission
        if(isUpdateRole.HasValue && isUpdateRole.Value) 
            userEntity.RoleId = userDto.RoleId;
        
        // Update user properties
        userEntity.FirstName = userDto.FirstName;
        userEntity.LastName = userDto.LastName;
        userEntity.Email = userDto.Email;
        userEntity.Phone = userDto.Phone;
        userEntity.Username = userDto.Username;
        userEntity.AvatarImage = userDto.AvatarImage;
        userEntity.DateOfBirth = userDto.DateOfBirth;
        userEntity.CreateDate = userDto.CreateDate;
        userEntity.TestTakenDate = userDto.TestTakenDate;
        userEntity.TargetScore = userDto.TargetScore;
        
        // Progress update 
        await unitOfWork.UserRepository.UpdateAsync(userEntity);
        return await unitOfWork.UserRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<bool> RemoveAsync(Guid userId, bool forceDelete = false)
    {
        if (forceDelete) // Check whether force delete 
        {
            // Get user by id 
            var userEntities = 
                await unitOfWork.UserRepository.FindAllWithConditionAndThenIncludeAsync(
                    filter: u => u.UserId == userId,
                    includes: new()
                    {
                        // Include user comments
                        query => query.Include(u => u.Comments),
                        // Include user test 
                        query => query.Include(u => u.Tests),
                        // Include user test histories
                        query => query.Include(u => u.TestHistories)
                    });
            
            // Check exist query 
            if(!userEntities.Any()) return false;
            // Get first user 
            var toDeleteUser = userEntities.First();
            
            // Remove user comments
            if (toDeleteUser!.Comments.Any())
            {
                // Remove range comments and their children comment 
                await commentService.RemoveRangeCommentAndChildrenAsync(
                    toDeleteUser.Comments.Adapt<List<CommentDto>>());
            }
            // Remove user tests
            if (toDeleteUser.Tests.Any())
            {
                foreach (var test in toDeleteUser.Tests)
                {
                    await testService.RemoveAsync(test.TestId);
                }
            }
            // Remove user test histories
            if (toDeleteUser.TestHistories.Any())
            {
                foreach (var testHistory in toDeleteUser.TestHistories)
                {
                    await testHistoryService.RemoveAsync(testHistory.TestHistoryId);
                }
            }
            
            // Perform remove user 
            await unitOfWork.UserRepository.RemoveAsync(userId);
            return await unitOfWork.UserRepository.SaveChangeWithTransactionAsync() > 0;
        }
        
        // Remove user by de-active
        await unitOfWork.UserRepository.RemoveByDeActiveAsync(userId);
        // Save change to DB
        return await unitOfWork.UserRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<bool> IsExistUserAsync(Guid userId)
    {
        return await unitOfWork.UserRepository.IsExistUserAsync(userId);
    }

    public async Task<bool> IsExistUsername(string username)
    {
        return await unitOfWork.UserRepository.IsExistUsername(username);
    }

    public Task<int> CountTotalAsync()
    {
        return unitOfWork.UserRepository.CountTotalAsync();
    }

    public Task<int> CountTotalActiveAsync()
    {
        return unitOfWork.UserRepository.CountTotalActiveAsync();
    }

    public Task<int> CountTotalInActiveAsync()
    {
        return unitOfWork.UserRepository.CountTotalInActiveAsync();
    }

    public async Task<UserDto?> FindOneWithConditionAsync(Expression<Func<User, bool>> filter, Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, string? includeProperties = "")
    {
        var userEntity = await unitOfWork.UserRepository.FindOneWithConditionAsync(filter, orderBy, includeProperties);   
        return userEntity.Adapt<UserDto>();
    }

    public async Task<List<UserDto>> FindAllWithConditionAsync(Expression<Func<User, bool>> filter, Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, string? includeProperties = "")
    {
        var userEntities = await unitOfWork.UserRepository.FindAllWithConditionAsync(
            filter, orderBy, includeProperties);
        return userEntities.Adapt<List<UserDto>>();
    }

    public async Task<UserDto?> GetUserByClerkId(string clerkId)
    {
        var user = await unitOfWork.UserRepository.FindUserByClerkId(clerkId);
        return user.Adapt<UserDto>();
    }

    public async Task<UserDto?> FindByEmailAsync(string email)
    {
        var userEntity = await unitOfWork.UserRepository.FindByEmailAsync(email);
        return userEntity.Adapt<UserDto>();
    }

    public async Task<IList<UserDto>> FindAllAsync()
    {
        var userEntities = await unitOfWork.UserRepository.FindAllAsync();
        return userEntities.Adapt<IList<UserDto>>();
    }

    public async Task<IList<UserDto>> FindAllInActiveAsync()
    {
        var userEntities = await unitOfWork.UserRepository.FindAllInActiveAsync();
        return userEntities.Adapt<IList<UserDto>>();
    }

    public async Task<IList<UserDto>> FindAllWithConditionAndPagingAsync(
        Expression<Func<User, bool>>? filter, 
        Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy, 
        string? includeProperties, int? pageIndex, int? pageSize)
    {
        var userEntities = await unitOfWork.UserRepository.FindAllWithConditionAndPagingAsync(
            filter, orderBy, includeProperties, pageIndex, pageSize);
        return userEntities.Adapt<IList<UserDto>>();
    }
}