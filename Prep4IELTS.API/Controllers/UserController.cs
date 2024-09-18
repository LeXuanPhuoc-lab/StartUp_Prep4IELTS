using EXE202_Prep4IELTS.Attributes;
using EXE202_Prep4IELTS.Payloads;
using EXE202_Prep4IELTS.Payloads.Requests;
using EXE202_Prep4IELTS.Payloads.Requests.Users;
using EXE202_Prep4IELTS.Payloads.Responses;
using Microsoft.AspNetCore.Mvc;
using Prep4IELTS.Business.Constants;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data.Dtos;

namespace EXE202_Prep4IELTS.Controllers;

[ApiController]
public class UserController(
    IUserService userService, 
    ISystemRoleService roleService) : ControllerBase
{
    [ClerkAuthorize]
    [HttpGet(ApiRoute.User.WhoAmI, Name = nameof(WhoAmI))]
    public IActionResult WhoAmI()
    {
        var user = (UserDto?)HttpContext.Items["User"];
        return Ok(user);
    }

    [ClerkAuthorize(Roles = [SystemRoleConstants.Admin])]
    [HttpGet(ApiRoute.User.GetAllUserRole, Name = nameof(GetAllUserRoleAsync))]
    public async Task<IActionResult> GetAllUserRoleAsync()
    {
        // Get all system role 
        var roleDtos = await roleService.FindAllAsync();
        
        return Ok(new BaseResponse()
        {
            StatusCode = StatusCodes.Status200OK,
            Data = roleDtos
        });
    }
    
    [ClerkAuthorize(Roles = [SystemRoleConstants.Admin])]
    [HttpPost(ApiRoute.User.Create, Name = nameof(CreateUserAsync))]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest req)
    {
        // Validations 
        if(!ModelState.IsValid) return UnprocessableEntity(ModelState);
        
        // Check exist role 
        var isExistRole = await roleService.IsExistRoleAsync(req.RoleId);
        if (!isExistRole) // Not existing role
        {
            ModelState.AddModelError("roleId", $"Role with id {req.RoleId} does not exist.");
            return UnprocessableEntity(ModelState);
        }
        
        // Check exist user 
        var userDto = await userService.FindByEmailAsync(req.Email);
        if (userDto != null)
        {
            ModelState.AddModelError("email", $"Email already exist.");
            return UnprocessableEntity(ModelState);
        }
        
        // Initiate user dto
        var toInsertUser = new UserDto(
            Id:0, UserId: Guid.Empty, ClerkId: string.Empty,
            FirstName: string.Empty, LastName: string.Empty,
            Email: req.Email, Username: string.Empty, Phone: null,
            AvatarImage: string.Empty, IsActive: null,
            DateOfBirth: null, CreateDate: DateTime.UtcNow,
            TestTakenDate: null, TargetScore: null, 
            // Default is [Student] role
            RoleId: req.RoleId,
            Role: null!);
        // Progress create new user 
        var isCreated = await userService.InsertAsync(toInsertUser);
        
        return isCreated
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong.");
    }

    [ClerkAuthorize(Roles =
    [
        SystemRoleConstants.Admin,
        SystemRoleConstants.Staff,
        SystemRoleConstants.Student
    ])]
    [HttpPost(ApiRoute.User.Update, Name = nameof(UpdateUserAsync))]
    public async Task<IActionResult> UpdateUserAsync([FromRoute] Guid userId, [FromBody] UpdateUserRequest req)
    {
        // Get user by id
        var userDto =
            await userService.FindOneWithConditionAsync(u => u.UserId == userId);

        // Check existing user
        if (userDto == null)
        {
            return BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found user match"
            });
        }
    
        // Progress update user
        var toUpdateUser = new UserDto(
                userDto.Id, userDto.UserId, userDto.ClerkId,
                FirstName: req.FirstName ?? userDto.FirstName, LastName: req.LastName ?? userDto.LastName,
                Email: userDto.Email, Username: req.Username ?? string.Empty, Phone: userDto.Phone,
                AvatarImage: userDto.AvatarImage, IsActive: userDto.IsActive,
                DateOfBirth: req.DateOfBirth ?? userDto.DateOfBirth, CreateDate: userDto.CreateDate,
                TestTakenDate: req.TestTakenDate ?? userDto.TestTakenDate, 
                TargetScore: req.TargetScore ?? userDto.TargetScore, 
                RoleId: userDto.RoleId, Role: userDto.Role);
        
        // Progress update user 
        var isUpdated = await userService.UpdateAsync(
            toUpdateUser, isUpdateClerkId: false, isUpdateRole: false);
        
        return isUpdated
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
    }

    // [ClerkAuthorize(Roles = [SystemRoleConstants.Admin])]
    // [HttpDelete(ApiRoute.User.Delete, Name = nameof(DeleteUserAsync))]
    // public async Task<IActionResult> DeleteUserAsync([FromRoute] Guid userId, [FromQuery] bool forceDelete = false)
    // {
    //     // Get user by id
    //     var userDto =
    //         await userService.FindOneWithConditionAsync(u => u.UserId == userId);
    //
    //     // Check existing user
    //     if (userDto == null)
    //     {
    //         return BadRequest(new BaseResponse()
    //         {
    //             StatusCode = StatusCodes.Status404NotFound,
    //             Message = "Not found user match"
    //         });
    //     }
    //     
    //     // Progress delete user 
    //     
    // }
    
}