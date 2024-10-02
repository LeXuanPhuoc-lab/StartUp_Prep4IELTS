using System.ComponentModel.DataAnnotations;
using EXE202_Prep4IELTS.Attributes;
using EXE202_Prep4IELTS.Payloads;
using EXE202_Prep4IELTS.Payloads.Filters;
using EXE202_Prep4IELTS.Payloads.Requests.Users;
using EXE202_Prep4IELTS.Payloads.Responses;
using EXE202_Prep4IELTS.Payloads.Responses.PremiumPackage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Prep4IELTS.Business.Constants;
using Prep4IELTS.Business.Models;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Business.Utils;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Enum;

namespace EXE202_Prep4IELTS.Controllers;

[ApiController]
public class UserController(
    IUserService userService, 
    ISystemRoleService roleService,
    IUserPremiumPackageService userPremiumPackageService,
    IOptionsMonitor<AppSettings> monitor) : ControllerBase
{
    private readonly AppSettings _appSettings = monitor.CurrentValue;
    
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
    [HttpGet(ApiRoute.User.GetAll, Name = nameof(GetAllUserAsync))]
    public async Task<IActionResult> GetAllUserAsync([FromQuery] UserFilterRequest req)
    {
        // Validations
        if(!ModelState.IsValid) return UnprocessableEntity(ModelState);
        
        // Initiate search term 
        var searchTerm = req.Term?.Trim() ?? string.Empty;
        
        // Get admin role 
        var adminRole = await roleService.FindByRoleNameAsync(SystemRole.Admin);
        
        // Progress get all with filter, order, and paging
        var userDtos = await userService.FindAllWithConditionAsync(
            filter: u => 
                ( // Optional fields
                    // Search with fullname
                    searchTerm.Contains(u.FirstName) || searchTerm.Contains(u.LastName) ||
                    // Search with email
                    u.Email.Contains(searchTerm) ||
                    // Search with phone number
                    (u.Phone != null && u.Phone.Contains(searchTerm))) && 
                // Required field
                // And user must be active
                u.IsActive == true && 
                // Not allow to see other admin
                u.RoleId != adminRole.RoleId, 
            orderBy: q => q.OrderBy(u => u.Id),
            includeProperties: "Role");
        
        
        // Sorting 
        if (!string.IsNullOrEmpty(req.OrderBy))
        {
            var sortingEnumerable = await SortHelper.SortUserByColumnAsync(userDtos, req.OrderBy);
            userDtos = sortingEnumerable.ToList();
        }

        // Total actual users
        // var actualTotal = await userService.CountTotalActiveAsync();
        
        // Create paginated detail list 
        var paginatedDetail = PaginatedList<UserDto>.Paginate(userDtos,
            pageIndex: req.Page ?? 1,
            req.PageSize ?? _appSettings.PageSize);

        return !userDtos.Any() // Not exist any user
            ? NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any users."
            })
            : Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    Users = paginatedDetail,
                    Page = paginatedDetail.PageIndex,
                    TotalPage = paginatedDetail.TotalPage
                }
            });
    }
    
    
    [ClerkAuthorize(Roles = [SystemRoleConstants.Admin])]
    [HttpGet(ApiRoute.User.GetAllInActive, Name = nameof(GetAllInActiveUserAsync))]
    public async Task<IActionResult> GetAllInActiveUserAsync([FromQuery] UserFilterRequest req)
    {
        // Validations
        if(!ModelState.IsValid) return UnprocessableEntity(ModelState);
        
        // Initiate search term 
        var searchTerm = req.Term?.Trim() ?? string.Empty;
        
        // Progress get all with filter, order, and paging
        var userDtos = await userService.FindAllWithConditionAsync(
            filter: u => 
                (
                    // Search with fullname
                    searchTerm.Contains(u.FirstName) || 
                    searchTerm.Contains(u.LastName) || 
                    // Search with email
                    u.Email.Contains(searchTerm) ||
                    // Search with phone number
                    (u.Phone != null && u.Phone.Contains(searchTerm))) && 
                // is not active user
                u.IsActive == false,
            orderBy: q => q.OrderBy(u => u.Id),
            includeProperties: "Role");
        
        
        // Sorting 
        if (!string.IsNullOrEmpty(req.OrderBy))
        {
            var sortingEnumerable = await SortHelper.SortUserByColumnAsync(userDtos, req.OrderBy);
            userDtos = sortingEnumerable.ToList();
        }

        // Total actual users
        // var actualTotal = await userService.CountTotalInActiveAsync();
        
        // Create paginated detail list 
        var paginatedDetail = PaginatedList<UserDto>.Paginate(userDtos,
            pageIndex: req.Page ?? 1,
            req.PageSize ?? _appSettings.PageSize);

        return !userDtos.Any() // Not exist any user
            ? NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any users."
            })
            : Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    Users = paginatedDetail,
                    Page = paginatedDetail.PageIndex,
                    TotalPage = paginatedDetail.TotalPage
                }
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
    [HttpPut(ApiRoute.User.Update, Name = nameof(UpdateUserAsync))]
    public async Task<IActionResult> UpdateUserAsync([FromRoute] Guid userId, [FromBody] UpdateUserRequest req)
    {
        // Validations
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
        
        // Check exist username
        if (!string.IsNullOrEmpty(req.Username))
        {
            var isExistUsername = await userService.IsExistUsername(req.Username);
            if (isExistUsername)
            {
                ModelState.AddModelError("username", "Username already exist.");
                return UnprocessableEntity(ModelState);
            }
        }
        
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
                Email: userDto.Email, Username: req.Username ?? userDto.Username, Phone: userDto.Phone,
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

    [ClerkAuthorize(Roles = [SystemRoleConstants.Admin])]
    [HttpPut(ApiRoute.User.AuthorizeUpdate, Name = nameof(AuthorizeUpdateUserAsync))]
    public async Task<IActionResult> AuthorizeUpdateUserAsync([FromRoute] Guid userId, [FromBody] UpdateUserRequest req)
    {
        // Get admin information
        var adminDto = HttpContext.Items["User"] as UserDto;
        if (adminDto == null) return Unauthorized();

        // Check if admin update themselves, then not allow to update role
        bool isUpdateRole = true;
        if (adminDto.UserId.Equals(userId) && !string.IsNullOrEmpty(req.RoleId.ToString()))
        {
            isUpdateRole = false;
            return BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Do not allow to update your own role"
            });
        }
        
        // Validations
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
        
        // Check exist username
        if (!string.IsNullOrEmpty(req.Username))
        {
            var isExistUsername = await userService.IsExistUsername(req.Username);
            if (isExistUsername)
            {
                ModelState.AddModelError("username", "Username already exist.");
                return UnprocessableEntity(ModelState);
            }
        }
        
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
            Email: userDto.Email, Username: req.Username ?? userDto.Username, Phone: userDto.Phone,
            AvatarImage: userDto.AvatarImage, IsActive: userDto.IsActive,
            DateOfBirth: req.DateOfBirth ?? userDto.DateOfBirth, CreateDate: userDto.CreateDate,
            TestTakenDate: req.TestTakenDate ?? userDto.TestTakenDate, 
            TargetScore: req.TargetScore ?? userDto.TargetScore, 
            // Update role (ADMIN Only)
            RoleId: req.RoleId ?? userDto.RoleId, Role: null!);
        
        // Progress update user 
        var isUpdated = await userService.UpdateAsync(
            toUpdateUser, isUpdateClerkId: false, isUpdateRole: isUpdateRole);
        
        return isUpdated
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
    }
    
    [ClerkAuthorize(Roles = [SystemRoleConstants.Admin])]
    [HttpDelete(ApiRoute.User.Delete, Name = nameof(DeleteUserAsync))]
    public async Task<IActionResult> DeleteUserAsync([FromRoute] Guid userId, [FromQuery] bool forceDelete = false)
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
        
        // Check if user already sign up in application
        // NOT allow to remove user is active, exist username when progressing force delete 
        if(forceDelete && (!string.IsNullOrEmpty(userDto?.ClerkId) 
                           || !string.IsNullOrEmpty(userDto?.Username)
                           || userDto?.IsActive == true))
        {
            return BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Not allow to remove (activated) user or user existing credential information"
            });
        }
        
        // Check user already de-active
        if (!forceDelete && userDto?.IsActive == false)
        {
            return BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "User is already inactive"
            });
        }
        
        // Progress delete user 
        var isRemoved = await userService.RemoveAsync(userId, forceDelete: forceDelete);
        
        return isRemoved
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
    }

    [Authorize]
    [HttpGet(ApiRoute.User.GetPremiumPackage, Name = nameof(GetUserPremiumPackageAsync))]
    public async Task<IActionResult> GetUserPremiumPackageAsync()
    {
        // Check exist user
        var userDto = HttpContext.Items["User"] as UserDto;
        if (userDto == null) return Unauthorized();
        
        // Get user premium package
        var userPremiumPackage = await userPremiumPackageService.FindUserPremiumPackageAsync(userDto.UserId);
    
        // Initiate user premium package
        var userPremiumPackageResp = new UserPremiumPackageResponse();
        if (userPremiumPackage != null)
        {
            var currentDatetime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, 
                TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
            
            userPremiumPackageResp = new UserPremiumPackageResponse()
            {
                PremiumPackageId = userPremiumPackage.PremiumPackageId,
                PremiumPackageName = userPremiumPackage.PremiumPackage.PremiumPackageName,
                CreateDate = userPremiumPackage.PremiumPackage.CreateDate,
                Description = userPremiumPackage.PremiumPackage.Description,
                ExpireDate = userPremiumPackage.ExpireDate,
                TotalTrials = userPremiumPackage.TotalTrials,
                IsPremiumActive = userPremiumPackage.ExpireDate >= currentDatetime && userPremiumPackage.IsActive,
            };
        }

        return Ok(new BaseResponse()
        {
            StatusCode = StatusCodes.Status200OK,
            Message = userPremiumPackageResp == null! 
                ? "User premium package is de-activated" 
                : string.Empty,
            Data = userPremiumPackageResp
        });
    }
}