using EXE202_Prep4IELTS.Attributes;
using EXE202_Prep4IELTS.Payloads;
using EXE202_Prep4IELTS.Payloads.Filters;
using EXE202_Prep4IELTS.Payloads.Requests.Flashcards;
using EXE202_Prep4IELTS.Payloads.Responses;
using EXE202_Prep4IELTS.Payloads.Responses.Flashcards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Prep4IELTS.Business.Models;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Business.Utils;
using Prep4IELTS.Data.Dtos;

namespace EXE202_Prep4IELTS.Controllers;

[ApiController]
public class FlashcardController(
    IFlashcardService flashcardService, 
    IOptionsMonitor<AppSettings> monitor) : ControllerBase
{
    private readonly AppSettings _appSettings = monitor.CurrentValue;

    #region Public Flashcard
    
    [HttpGet(ApiRoute.Flashcard.GetAll, Name = nameof(GetAllFlashcardAsync))]
    public async Task<IActionResult> GetAllFlashcardAsync([FromQuery] FlashcardFilterRequest req)
    {
        // Validations
        if(!ModelState.IsValid) return UnprocessableEntity(ModelState);
        
        // Initiate search term 
        var searchTerm = req.Term?.Trim() ?? string.Empty;
        
        // Progress get all with filter, order, and paging
        var flashcardDtos = await flashcardService.FindAllWithConditionAsync(
            filter: u => 
                // Search with title
                u.Title.Contains(searchTerm) && u.IsPublic,
            orderBy: q => q.OrderBy(u => u.TotalView),
            includeProperties: null);
        
        // Sorting 
        if (!string.IsNullOrEmpty(req.OrderBy))
        {
            var sortingEnumerable = await flashcardDtos.SortFlashcardByColumnAsync(req.OrderBy);
            flashcardDtos = sortingEnumerable.ToList();
        }

        // Create paginated detail list 
        var paginatedDetail = PaginatedList<FlashcardDto>.Paginate(flashcardDtos,
            pageIndex: req.Page ?? 1,
            req.PageSize ?? _appSettings.PageSize);

        return !flashcardDtos.Any() // Not exist any flashcard
            ? NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any flashcards."
            })
            : Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    Flashcards = paginatedDetail,
                    Page = paginatedDetail.PageIndex,
                    TotalPage = paginatedDetail.TotalPage
                }
            });
    }

    [HttpGet(ApiRoute.Flashcard.GetById, Name = nameof(GetFlashcardByIdAsync))]
    public async Task<IActionResult> GetFlashcardByIdAsync([FromRoute] int id)
    {
        var flashcardDto = await flashcardService.FindByIdAsync(id);

        return Ok(new BaseResponse()
        {
            StatusCode = StatusCodes.Status200OK,
            Message = "Get data successfully",
            Data = flashcardDto
        });
    }

    [ClerkAuthorize]
    [HttpPost(ApiRoute.Flashcard.AddUser, Name = nameof(AddUserToFlashcardAsync))]
    public async Task<IActionResult> AddUserToFlashcardAsync(
        [FromRoute] int id)
    {
        // Check exist user 
        var userDto = HttpContext.Items["User"] as UserDto;
        if (userDto == null) return Unauthorized();
        
        // Check flashcard exist
        var isExistFlashcard = await flashcardService.IsExistAsync(id);
        if (!isExistFlashcard)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any flashcard match"
            });
        }
        
        // Check exist user flashcard
        var isExistUserFlashcard = await flashcardService.IsExistUserFlashcardAsync(id, userDto.UserId);
        if (isExistUserFlashcard)
        {
            return BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "User already possess this flashcard. Cannot add more"
            });
        }
        
        // Check exist user
        var isAddSuccess = await flashcardService.AddFlashcardToUserAsync(id, userDto.UserId);
        return isAddSuccess 
            ? NoContent() 
            : StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong.");
    }
    
    #endregion

    #region Privacy Flashcard

    [ClerkAuthorize]
    [HttpGet(ApiRoute.Flashcard.GetAllPrivacy, Name = nameof(GetAllPrivacyFlashcardAsync))]
    public async Task<IActionResult> GetAllPrivacyFlashcardAsync([FromQuery] FlashcardFilterRequest req)
    {
        // Validations
        if(!ModelState.IsValid) return UnprocessableEntity(ModelState);
        
        // Check exits user
        var userDto = HttpContext.Items["User"] as UserDto;
        if(userDto == null) return Unauthorized();
        
        // Initiate search term 
        var searchTerm = req.Term?.Trim() ?? string.Empty;
        
        // Progress get all with filter, order, and paging
        var flashcardDtos = await flashcardService.FindAllPrivacyWithConditionAsync(
            filter: u => 
                // Search with title
                u.Title.Contains(searchTerm) && 
                u.UserFlashcards.Any(uf => uf.UserId == userDto.UserId),
            orderBy: q => q.OrderBy(u => u.TotalView),
            includeProperties: null,
            userId: userDto.UserId);
        
        // Sorting 
        if (!string.IsNullOrEmpty(req.OrderBy))
        {
            var sortingEnumerable = await flashcardDtos.SortFlashcardByColumnAsync(req.OrderBy);
            flashcardDtos = sortingEnumerable.ToList();
        }

        // Calculate user flashcard progress
        List<FlashcardResponse> flashcardResponses = new();
        foreach (var fCard in flashcardDtos)
        {
            var flashcardProgressData = 
                await flashcardService.CalculateUserFlashcardProgressAsync(fCard.FlashcardId, userDto.UserId);
            flashcardResponses.Add(await fCard.ToPrivacyFlashcardResponseAsync(
                flashcardProgressData.totalNew,
                flashcardProgressData.totalStudying,
                flashcardProgressData.totalProficient,
                flashcardProgressData.totalStarred));
        }
        
        // Create paginated detail list 
        var paginatedDetail = PaginatedList<FlashcardResponse>.Paginate(flashcardResponses,
            pageIndex: req.Page ?? 1,
            req.PageSize ?? _appSettings.PageSize);
        
        var userFlashcardProgress = await flashcardService.CalculateAllUserFlashcardProgressAsync(userDto.UserId);
        
        return !flashcardDtos.Any() // Not exist any flashcard
            ? NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any flashcards."
            })
            : Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    FlashcardProgress = userFlashcardProgress,
                    Flashcards = paginatedDetail,
                    Page = paginatedDetail.PageIndex,
                    TotalPage = paginatedDetail.TotalPage
                }
            });
    }
    
    [ClerkAuthorize]
    [HttpGet(ApiRoute.Flashcard.GetByIdPrivacy, Name = nameof(GetPrivacyFlashcardAsync))]
    public async Task<IActionResult> GetPrivacyFlashcardAsync([FromRoute] int id)
    {
        // Check exist user 
        var userDto = HttpContext.Items["User"] as UserDto;
        if (userDto == null) return Unauthorized();
        
        var flashcardDto = await flashcardService.FindByIdAsync(id, userDto.UserId);

        if (flashcardDto == null)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found user in this flashcard"
            });
        }
        
        // Select many flashcard progress
        var flashcardProgresses = 
            flashcardDto.UserFlashcards.SelectMany(uf => uf.UserFlashcardProgresses).ToList();
        
        // Customize flashcard for user resp
        var flashcardResp = await flashcardDto.ToPrivacyFlashcardResponseAsync(
            flashcardProgresses);
        
        return Ok(new BaseResponse()
        {
            StatusCode = StatusCodes.Status200OK,
            Message = "Get data successfully",
            Data = flashcardResp
        });
    }
    
    [ClerkAuthorize]
    [HttpPost(ApiRoute.Flashcard.PrivacyCreate, Name = nameof(CreatePrivacyFlashcardAsync))]
    public async Task<IActionResult> CreatePrivacyFlashcardAsync([FromBody] CreatePrivacyFlashcardRequest req)
    {
        if(!ModelState.IsValid) return UnprocessableEntity(ModelState);
        
        // Check exist user 
        var userDto = HttpContext.Items["User"] as UserDto;
        if (userDto == null) return Unauthorized();
        
        // Map privacy flashcard request to flashcardDto
        var flashcardDto = req.ToFlashcardDto();
        
        // Progress create new privacy flashcard
        var isCreated = await flashcardService.InsertPrivacyAsync(flashcardDto, userDto.UserId);
        
        return isCreated
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong.");
    }
    
    #endregion
}