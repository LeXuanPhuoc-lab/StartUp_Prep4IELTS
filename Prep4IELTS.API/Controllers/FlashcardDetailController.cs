using EXE202_Prep4IELTS.Attributes;
using EXE202_Prep4IELTS.Payloads;
using EXE202_Prep4IELTS.Payloads.Requests.Flashcards;
using EXE202_Prep4IELTS.Payloads.Responses;
using Microsoft.AspNetCore.Mvc;
using Prep4IELTS.Business.Constants;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Enum;
using Prep4IELTS.Data.Extensions;

namespace EXE202_Prep4IELTS.Controllers;

[ApiController]
public class FlashcardDetailController(
    IFlashcardService flashcardService,
    ICloudinaryService cloudinaryService,
    IFlashcardDetailService flashcardDetailService) : ControllerBase
{
    #region Admin-Staff
    
    [ClerkAuthorize(Roles = [SystemRoleConstants.Admin, SystemRoleConstants.Staff])]
    [HttpPost(ApiRoute.FlashcardDetail.Create, Name = nameof(CreateFlashcardDetailAsync))]
    public async Task<IActionResult> CreateFlashcardDetailAsync(
        [FromForm] CreateFlashcardDetailRequest req)
    {
        // Validation
        if(!ModelState.IsValid) return UnprocessableEntity(ModelState);
        
        // Check exist flashcard 
        var isExistFlashcard = await flashcardService.IsExistAsync(req.FlashcardId);
        if(!isExistFlashcard) return NotFound(new BaseResponse()
        {
            StatusCode = StatusCodes.Status404NotFound,
            Message = "Not found any flashcard match"
        });

        string publicId = string.Empty;
        string? imageUrl = string.Empty;
        int? fileBytes = 0;
        // Check exist image file 
        if (req.Image != null)
        {
            // Assign file byte
            fileBytes = (int)req.Image.Length;
            
            // Progress upload image to cloud
            publicId = $"flashcards/{req.WordText}";
            
            // Upload image to cloudinary
            var result = await cloudinaryService.UploadAsync(publicId, req.Image, FileType.Image);
        
            // Check whether exist secureUrl and null error message
            var isUploadSuccess = string.IsNullOrEmpty(result.messageErr) && !string.IsNullOrEmpty(result.secureUrl);
            
            if(isUploadSuccess) imageUrl = result.secureUrl;
        }
        
        // Progress add new flashcard detail
        var isCreated = await flashcardDetailService.InsertAsync(req.FlashcardId, 
            // Convert to flashcard dto
            req.ToFlashcardDetailDto(publicId, imageUrl, fileBytes));

        return isCreated 
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
    }
    
    #endregion
    
    #region Privacy user

    [ClerkAuthorize]
    [HttpPost(ApiRoute.FlashcardDetail.PrivacyCreate, Name = nameof(CreatePrivacyFlashcardDetailAsync))]
    public async Task<IActionResult> CreatePrivacyFlashcardDetailAsync(
        [FromForm] CreateFlashcardDetailRequest req)
    {
        // Validation
        if(!ModelState.IsValid) return UnprocessableEntity(ModelState);
        
        // Check exist user
        var userDto = HttpContext.Items["User"] as UserDto;
        if (userDto == null) return Unauthorized();
        
        // Check exist flashcard 
        var isExistFlashcard = await flashcardService.IsExistAsync(req.FlashcardId);
        if(!isExistFlashcard) return NotFound(new BaseResponse()
        {
            StatusCode = StatusCodes.Status404NotFound,
            Message = "Not found flashcard match"
        });
        
        // Check is public flashcard
        var isPublicFlashcard = await flashcardService.IsPublicAsync(req.FlashcardId);
        if (isPublicFlashcard)
        {
            return BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Not allow to create detail for this flashcard"
            });
        }
                
        string publicId = string.Empty;
        string? imageUrl = string.Empty;
        int? fileBytes = 0;
        // Check exist image file 
        if (req.Image != null)
        {
            // Assign file byte
            fileBytes = (int)req.Image.Length;
            
            // Progress upload image to cloud
            publicId = $"flashcards/{req.WordText}";
            
            // Upload image to cloudinary
            var result = await cloudinaryService.UploadAsync(publicId, req.Image, FileType.Image);
        
            // Check whether exist secureUrl and null error message
            var isUploadSuccess = string.IsNullOrEmpty(result.messageErr) && !string.IsNullOrEmpty(result.secureUrl);
            
            if(isUploadSuccess) imageUrl = result.secureUrl;
        }
        
        // Progress add new flashcard detail
        var isCreated = await flashcardDetailService.InsertPrivacyAsync(req.FlashcardId, userDto.UserId,
            // Convert to flashcard dto
            req.ToFlashcardDetailDto(publicId, imageUrl, fileBytes));

        return isCreated 
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
    }
    
    [ClerkAuthorize]
    [HttpPut(ApiRoute.FlashcardDetail.PrivacyUpdate, Name = nameof(UpdatePrivacyFlashcardDetailAsync))]
    public async Task<IActionResult> UpdatePrivacyFlashcardDetailAsync([FromRoute] int id,
        [FromForm] UpdateFlashcardDetailRequest req)
    {
        if(!ModelState.IsValid) return UnprocessableEntity(ModelState);

        // Check exist flashcard detail id
        var toUpdateFlashcard = await flashcardDetailService.FindByIdAsync(id);
        if (toUpdateFlashcard == null)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Not found any flashcard detail match"
            });
        }
        
        // Check is public flashcard
        var isPublicFlashcard = await flashcardDetailService.IsInPublicFlashcard(id);
        if (isPublicFlashcard)
        {
            return BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Not allow to update this flashcard detail"
            });
        }
        
        string publicId = string.Empty;
        string? imageUrl = string.Empty;
        int? fileBytes = 0;
        // Check whether update flashcard detail image
        if (req.Image != null)
        {
            // Assign file bytes
            fileBytes = (int)req.Image.Length;
            
            // Remove existing image
            var cloudResource = toUpdateFlashcard.CloudResource;
            if (cloudResource != null
                && !string.IsNullOrEmpty(cloudResource.PublicId))
            {
                await cloudinaryService.DeleteAsync(cloudResource.PublicId, FileType.Image.GetDescription());
            }
            
            // Progress upload image to cloud
            publicId = $"flashcards/{req.WordText}-{Guid.NewGuid().ToString()}";
            
            // Upload image to cloudinary
            var result = await cloudinaryService.UploadAsync(publicId, req.Image, FileType.Image);
            
            // Upload new image
            // Check whether exist secureUrl and null error message
            var isUploadSuccess = string.IsNullOrEmpty(result.messageErr) && !string.IsNullOrEmpty(result.secureUrl);
            
            if(isUploadSuccess) imageUrl = result.secureUrl;
        }
        
        // Progress update flashcard
        // Progress add new flashcard detail
        await flashcardDetailService.UpdateAsync(
            // Convert to flashcard dto
            req.ToFlashcardDetailDto(id, publicId, imageUrl, fileBytes));

        return NoContent();
    }

    [ClerkAuthorize]
    [HttpDelete(ApiRoute.FlashcardDetail.PrivacyDelete, Name = nameof(DeletePrivacyFlashcardDetailAsync))]
    public async Task<IActionResult> DeletePrivacyFlashcardDetailAsync([FromRoute] int id)
    {
        // Check exist flashcard detail id
        var toDeleteFlashcard = await flashcardDetailService.FindByIdAsync(id);
        if (toDeleteFlashcard == null)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Not found any flashcard detail match"
            });
        }
        
        // Check is public flashcard
        var isPublicFlashcard = await flashcardDetailService.IsInPublicFlashcard(id);
        if (isPublicFlashcard)
        {
            return BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Not allow to delete this flashcard detail"
            });
        }
        
        // Progress delete flashcard
        var isRemoved = await flashcardDetailService.RemoveAsync(id);
        
        return isRemoved
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }
    
    #endregion
}