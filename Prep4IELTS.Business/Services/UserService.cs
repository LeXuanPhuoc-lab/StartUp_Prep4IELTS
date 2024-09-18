using System.Linq.Expressions;
using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services;

public class UserService(UnitOfWork unitOfWork) : IUserService
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
        await Task.CompletedTask;
        return true;
    }

    public async Task<bool> IsExistUserAsync(Guid userId)
    {
        return await unitOfWork.UserRepository.IsExistUserAsync(userId);
    }

    public async Task<UserDto?> FindOneWithConditionAsync(Expression<Func<User, bool>> filter, Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, string? includeProperties = "")
    {
        var userEntity = await unitOfWork.UserRepository.FindOneWithConditionAsync(filter, orderBy, includeProperties);   
        return userEntity.Adapt<UserDto>();
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
}