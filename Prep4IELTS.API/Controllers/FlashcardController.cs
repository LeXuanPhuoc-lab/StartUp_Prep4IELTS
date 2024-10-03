using System.ComponentModel.DataAnnotations;
using EXE202_Prep4IELTS.Attributes;
using EXE202_Prep4IELTS.Payloads;
using EXE202_Prep4IELTS.Payloads.Filters;
using EXE202_Prep4IELTS.Payloads.Requests;
using EXE202_Prep4IELTS.Payloads.Requests.Flashcards;
using EXE202_Prep4IELTS.Payloads.Responses;
using EXE202_Prep4IELTS.Payloads.Responses.Flashcards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Prep4IELTS.Business.Constants;
using Prep4IELTS.Business.Models;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Business.Utils;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Enum;
using Prep4IELTS.Data.Extensions;
using Svix.Model;

namespace EXE202_Prep4IELTS.Controllers;

[ApiController]
public class FlashcardController(
    IFlashcardService flashcardService,
    IUserFlashcardService userFlashcardService,
    IFlashcardExamHistoryService flashcardExamHistoryService,
    IOptionsMonitor<AppSettings> monitor) : ControllerBase
{
    private readonly AppSettings _appSettings = monitor.CurrentValue;

    #region Public Flashcard

    [ClerkAuthorize(Require = false)]
    [HttpGet(ApiRoute.Flashcard.GetAll, Name = nameof(GetAllFlashcardAsync))]
    public async Task<IActionResult> GetAllFlashcardAsync([FromQuery] FlashcardFilterRequest req)
    {
        // Get user (if any) from token
        var userDto = HttpContext.Items["User"] as UserDto;
        
        // Validations
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        // Initiate search term 
        var searchTerm = req.Term?.Trim() ?? string.Empty;

        // Progress get all with filter, order, and paging
        var flashcardDtos = await flashcardService.FindAllWithConditionForUserAsync(
            filter: u =>
                // Search with title
                u.Title.Contains(searchTerm) && u.IsPublic,
            orderBy: q => q.OrderBy(u => u.TotalView),
            includeProperties: null,
            userId: userDto?.UserId);

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
        // Get flashcard by id 
        var flashcardDto = await flashcardService.FindByIdAsync(id);

        // Update flashcard total view
        await flashcardService.UpdateFlashcardTotalViewAsync(id);
        
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
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        // Check exits user
        var userDto = HttpContext.Items["User"] as UserDto;
        if (userDto == null) return Unauthorized();

        // Initiate search term 
        var searchTerm = req.Term?.Trim() ?? string.Empty;

        // Progress get all with filter, order, and paging
        var flashcardDtos = await flashcardService.FindAllWithConditionForUserAsync(
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
        List<FlashcardResponseResponse> flashcardResponses = new();
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
        var paginatedDetail = PaginatedList<FlashcardResponseResponse>.Paginate(flashcardResponses,
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

        // Check exist user flashcard 
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
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

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

    [ClerkAuthorize]
    [HttpDelete(ApiRoute.Flashcard.PrivacyDelete, Name = nameof(DeletePrivacyFlashcardAsync))]
    public async Task<IActionResult> DeletePrivacyFlashcardAsync([FromRoute] int id)
    {
        // Check exist user 
        var userDto = HttpContext.Items["User"] as UserDto;
        if (userDto == null) return Unauthorized();

        // Check exist user flashcard
        var flashcardDto = await flashcardService.FindByIdAsync(id, userDto.UserId);
        if (flashcardDto == null)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found user in this flashcard"
            });
        }
        
        // Progress delete privacy flashcard
        var isRemoved = await flashcardService.RemovePrivacyAsync(id, userDto.UserId);
        
        return isRemoved
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong.");
    }

    [ClerkAuthorize]
    [HttpPut(ApiRoute.Flashcard.PrivacyUpdate, Name = nameof(UpdatePrivacyFlashcardAsync))]
    public async Task<IActionResult> UpdatePrivacyFlashcardAsync([FromRoute] int id, [FromBody] UpdatePrivacyFlashcardRequest req)
    {
        // Check exist user 
        var userDto = HttpContext.Items["User"] as UserDto;
        if (userDto == null) return Unauthorized();

        // Check exist user flashcard
        var flashcardDto = await flashcardService.FindByIdAsync(id, userDto.UserId);
        if (flashcardDto == null)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found user in this flashcard"
            });
        }
        
        // Check whether is public flashcard
        var isPublicFlashcard = flashcardDto.IsPublic;
        if (!isPublicFlashcard)
        {
            // Progress update privacy flashcard
            await flashcardService.UpdateAsync(
                new FlashcardDto(
                    FlashcardId: id, 
                    Title: req.Title,
                    TotalWords: 0,
                    TotalView: 0,
                    Description: req.Description,
                    CreateDate: null,
                    IsPublic: false,
                    FlashcardDetails: null!,
                    UserFlashcards: null!));

            return NoContent();
        }
        
        return BadRequest(new BaseResponse()
        {
            StatusCode = StatusCodes.Status400BadRequest,
            Message = "You are not allowed to update this flashcard"
        });
    }
    
    [HttpGet(ApiRoute.Flashcard.GetAllFlashcardStatus, Name = nameof(GetAllFlashcardStatusAsync))]
    public async Task<IActionResult> GetAllFlashcardStatusAsync()
    {
        List<string> flashcardStatuses = new()
        {
            FlashcardProgressStatus.New.GetDescription(),
            FlashcardProgressStatus.Studying.GetDescription(),
            FlashcardProgressStatus.Proficient.GetDescription(),
            FlashcardProgressStatus.Starred.GetDescription(),
        };

        return await Task.FromResult(Ok(new BaseResponse()
        {
            StatusCode = StatusCodes.Status200OK,
            Data = flashcardStatuses
        }));
    }

    [ClerkAuthorize]
    [HttpGet(ApiRoute.Flashcard.Practice, Name = nameof(PracticeFlashcardAsync))]
    public async Task<IActionResult> PracticeFlashcardAsync([FromRoute] int id, 
        [FromQuery] bool isTrackProgress,
        [FromQuery] List<FlashcardProgressStatus>? status)
    {
        // Check exist user 
        var userDto = HttpContext.Items["User"] as UserDto;
        if (userDto == null) return Unauthorized();
    
        // Check exist user flashcard
        var flashcardDto = await flashcardService.FindByIdAsync(id, userDto.UserId);
        if (flashcardDto == null)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found user in this flashcard"
            });
        }
        
        // Initiate user flashcard response
        UserFlashcardDto? userFlashcard = null;

        if (status != null && status.Any())
        {
            userFlashcard = await userFlashcardService.FindUserPracticingProgressWithStatusAsync(
                id, userDto.UserId, status);
        }
        else
        {
            userFlashcard = await userFlashcardService.FindUserPracticingProgressAsync(
                id, userDto.UserId, isTrackProgress);
        }
        
        return userFlashcard != null
            ? Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = userFlashcard
            })
            : NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found flashcard progresses"
            });
    }

    [ClerkAuthorize]
    [HttpPatch(ApiRoute.Flashcard.UpdateFlashcardProgress, Name = nameof(UpdateFlashcardProgressAsync))]
    public async Task<IActionResult> UpdateFlashcardProgressAsync([FromRoute] int id, 
        [FromQuery] int userFlashcardProgressId,[FromQuery] string status)
    {
        // Check exist user 
        var userDto = HttpContext.Items["User"] as UserDto;
        if (userDto == null) return Unauthorized();

        // Check exist user flashcard
        var flashcardDto = await flashcardService.FindByIdAsync(id, userDto.UserId);
        if (flashcardDto == null)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found user in this flashcard"
            });
        }

        switch (status)
        {
            case FlashcardProgressConstants.Studying:
                await flashcardService.UpdateUserFlashcardProgressStatusAsync(userFlashcardProgressId, FlashcardProgressStatus.Studying);                
                break;
            case FlashcardProgressConstants.Starred:
                await flashcardService.UpdateUserFlashcardProgressStatusAsync(userFlashcardProgressId, FlashcardProgressStatus.Starred);                
                break;
        }

        return NoContent();
    }

    [ClerkAuthorize]
    [HttpPatch(ApiRoute.Flashcard.Reset, Name = nameof(ResetFlashcardProgressAsync))]
    public async Task<IActionResult> ResetFlashcardProgressAsync([FromRoute] int id)
    {
        // Check exist user 
        var userDto = HttpContext.Items["User"] as UserDto;
        if (userDto == null) return Unauthorized();

        // Check exist user flashcard
        var flashcardDto = await flashcardService.FindByIdAsync(id, userDto.UserId);
        if (flashcardDto == null)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found user in this flashcard"
            });
        }
        
        // Progress reset flashcard
        await userFlashcardService.ResetFlashcardProgressAsync(id, userDto.UserId);
        
        // Response 
        return NoContent();
    }
    
    #endregion

    #region Premium Only

    // TRUE/FALSE, WRITTEN, MATCHING
    [ClerkAuthorize]
    [PremiumAuthorize(Types = [PremiumPackageType.Standard], AllowPremiumTrial = true)]
    [HttpGet(ApiRoute.Flashcard.Exam, Name = nameof(DoFlashcardExamAsync))]
    public async Task<IActionResult> DoFlashcardExamAsync([FromRoute] int id, 
        [FromQuery] ExamFlashcardFilterRequest req)
    {
        // Validation
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
        
        // Check exist user 
        var userDto = HttpContext.Items["User"] as UserDto;
        if (userDto == null) return Unauthorized();
        
        // Check exist user flashcard 
        var flashcardDto = await flashcardService.FindByIdAsync(id, userDto.UserId);
        if (flashcardDto == null || !flashcardDto.UserFlashcards.Any())
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found user in this flashcard"
            });
        }
        
        // Check exist flashcard details
        if (!flashcardDto.FlashcardDetails.Any() 
            // At least 3 details to create exam
            || flashcardDto.FlashcardDetails.Count < 3)
        {
            // Response not found 
            return BadRequest(new BaseResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Flashcard must at least 3 details to create an exam"
            });
        }
        
        // Generate flashcard exam questions
        var flashcardExamQuestions =
            flashcardDto.FlashcardDetails.ToList().GenerateFlashcardExamQuestions(
                totalQuestion: req.TotalQuestion,
                isTermPattern: req.IsTermPattern,
                questionTypes: req.QuestionTypes);
        
        return flashcardExamQuestions.Any()
            ? Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = flashcardExamQuestions
            })
            : StatusCode(StatusCodes.Status500InternalServerError);
    }


    [ClerkAuthorize]
    [PremiumAuthorize(Types = [PremiumPackageType.Standard], AllowPremiumTrial = true, UpdateTrialValue = true)]
    [HttpPost(ApiRoute.Flashcard.ExamSubmission, Name = nameof(FlashcardExamSubmissionAsync))]
    public async Task<IActionResult> FlashcardExamSubmissionAsync([FromBody] FlashcardExamSubmissionRequest req)
    {
        // Check exist user 
        var userDto = HttpContext.Items["User"] as UserDto;
        if (userDto == null) return Unauthorized();
    
        // Check exist user flashcard 
        var userFlashcardDto = await userFlashcardService.FindByUserAndFlashcardIdAsync(req.FlashcardId, userDto.UserId);
        if (userFlashcardDto == null!)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any user in this flashcard to progress test submission"
            });
        }
        
        // Map flashcard exam submission request to flashcard exam history
        var flashcardHistoryDto = req.ToFlashcardExamHistoryDto(userFlashcardDto.UserFlashcardId);
        
        // Progress add new flashcard exam history 
        var isInserted = await flashcardExamHistoryService.InsertAsync(
            flashcardExamHistoryDto: flashcardHistoryDto,
            userFlashcardId: userFlashcardDto.UserFlashcardId,
            isTermPattern: req.IsTermPattern,
            isSaveWrongToVocabSchedule: req.IsSaveWrongToVocabSchedule);
        
        return isInserted
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }


    [ClerkAuthorize]
    [PremiumAuthorize(Types = [PremiumPackageType.Standard], AllowPremiumTrial = true)]
    [HttpGet(ApiRoute.Flashcard.ExamResult, Name = nameof(GetLatestFlashcardExamResultAsync))]
    public async Task<IActionResult> GetLatestFlashcardExamResultAsync(
        [FromRoute] int id,
        [FromQuery] DateTime takenDateTime)
    {
        // Check exist user 
        var userDto = HttpContext.Items["User"] as UserDto;
        if(userDto == null) return Unauthorized();
        
        // Check exist user flashcard 
        var userFlashcardDto = await userFlashcardService.FindByUserAndFlashcardIdAsync(id, userDto.UserId);
        if (userFlashcardDto == null!)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any user in this flashcard to progress showing exam result"
            });
        }
        
        // Get user exam history
        var userFlashcardExamHisDto =
            await flashcardExamHistoryService.FindByUserFlashcardIdAtTakenDateAsync(userFlashcardDto.UserFlashcardId,
                takenDateTime);

        if (userFlashcardExamHisDto == null)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message =
                    "Not found user exam result, please check for the correctness of flashcardId and taken datetime"
            });
        }
        
        // Custom flashcard exam result 
        var flashcardExamHistoryResp =
            userFlashcardExamHisDto.ToFlashcardExamHistoryResponse();

        return Ok(new BaseResponse()
        {
            StatusCode = StatusCodes.Status200OK,
            Data = flashcardExamHistoryResp
        });
    }
    
    #endregion
}