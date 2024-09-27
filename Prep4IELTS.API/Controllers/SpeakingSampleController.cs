using CloudinaryDotNet;
using EXE202_Prep4IELTS.Attributes;
using EXE202_Prep4IELTS.Payloads;
using EXE202_Prep4IELTS.Payloads.Filters;
using EXE202_Prep4IELTS.Payloads.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Prep4IELTS.Business.Models;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Business.Utils;
using Prep4IELTS.Data.Dtos;

namespace EXE202_Prep4IELTS.Controllers;

[ApiController]
public class SpeakingSampleController(
    ISpeakingSampleService speakingSampleService,
    IOptionsMonitor<AppSettings> monitor) : ControllerBase
{
    private readonly AppSettings _appSettings = monitor.CurrentValue;
    
    [HttpGet(ApiRoute.SpeakingSample.GetAll, Name = nameof(GetAllSpeakingSampleAsync))]
    [ClerkAuthorize]
    public async Task<IActionResult> GetAllSpeakingSampleAsync([FromQuery] SpeakingSampleFilterRequest req)
    {
        // Get user 
        var userDto = HttpContext.Items["User"] as UserDto;
        if (userDto == null) return Unauthorized();
        
        // Validations
        if(!ModelState.IsValid) return UnprocessableEntity(ModelState);
        
        // Initiate search term 
        var searchTerm = req.Term?.Trim() ?? string.Empty;
        
        // Progress get all with filter, order, and paging
        var speakingSampleDtos = await speakingSampleService.FindAllWithConditionAsync(
            filter: u => 
                // Search with speaking sample name
                u.SpeakingSampleName.Contains(searchTerm) &&
                // Required field
                // And user must be active
                u.IsActive == true, 
            orderBy: null,
            includeProperties: "SpeakingParts",
            userId: userDto.UserId);
        
        
        // Sorting 
        if (!string.IsNullOrEmpty(req.OrderBy))
        {
            var sortingEnumerable = await SortHelper.SortSpeakingSampleByColumnAsync(speakingSampleDtos, req.OrderBy);
            speakingSampleDtos = sortingEnumerable.ToList();
        }

        // Total actual users
        // var actualTotal = await speakingSampleService.CountTotalAsync();
        
        // Create paginated detail list 
        var paginatedDetail = PaginatedList<SpeakingSampleDto>.Paginate(speakingSampleDtos,
            pageIndex: req.Page ?? 1,
            req.PageSize ?? _appSettings.PageSize);

        return !speakingSampleDtos.Any() // Not exist any user
            ? NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any speaking sample."
            })
            : Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    SpeakingSamples = paginatedDetail,
                    Page = paginatedDetail.PageIndex,
                    TotalPage = paginatedDetail.TotalPage
                }
            });
    }

    [HttpGet(ApiRoute.SpeakingSample.GetById, Name = nameof(GetSpeakingSampleByIdAsync))]
    [ClerkAuthorize]
    public async Task<IActionResult> GetSpeakingSampleByIdAsync([FromRoute] int id, [FromQuery] int[]? speakingPartId)
    {
        // Get user 
        var userDto = HttpContext.Items["User"] as UserDto;
        if (userDto == null) return Unauthorized();
        
        var speakingPart = 
            await speakingSampleService.FindAsync(id, userDto.UserId, speakingPartId);

        return speakingPart != null
            ? Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = speakingPart
            })
            : NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Not found any speaking match id {id}"
            });
    }

    
    [HttpPost(ApiRoute.SpeakingSample.AddUserSpeakingSampleHistory, Name = nameof(AddUserSpeakingSampleHistoryAsync))]
    [ClerkAuthorize]
    public async Task<IActionResult> AddUserSpeakingSampleHistoryAsync([FromRoute] int id)
    {
        // Get user 
        var userDto = HttpContext.Items["User"] as UserDto;
        if (userDto == null) return Unauthorized();
        
        // Check exist user speaking sample history
        var isExistUserSampleHistory =
            await speakingSampleService.IsExistUserSpeakingSampleHistoryAsync(id, userDto.UserId);
        if (isExistUserSampleHistory)
        {
            return BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "User already exists speaking sample history."
            });
        }
        
        var isCreateSuccess = 
            await speakingSampleService.InsertUserSpeakingSampleHistoryAsync(id, userDto.UserId);
        
        return isCreateSuccess
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }
}