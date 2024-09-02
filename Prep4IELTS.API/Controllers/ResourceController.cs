using System.Net;
using EXE202_Prep4IELTS.Payloads;
using EXE202_Prep4IELTS.Payloads.Requests.Resources;
using EXE202_Prep4IELTS.Payloads.Responses;
using Microsoft.AspNetCore.Mvc;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data.Enum;

namespace EXE202_Prep4IELTS.Controllers;

[ApiController]
public class ResourceController(ICloudinaryService cloudinaryService) : ControllerBase
{
    [HttpPost(ApiRoute.Resource.UploadImage)]
    public async Task<IActionResult> UploadImageAsync([FromForm] UploadImageRequest req)
    {
        // Check validation errors
        if (!ModelState.IsValid)
            // Return [422] with validation error details
            return new ObjectResult(StatusCodes.Status422UnprocessableEntity)
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity,
                Value = ModelState
            };
        
        // Custom for public Id
        var testType = req.TestType.ToLower();
        var customTestTitle = 
            req.TestTitle.Replace(" ", "_").ToLower();
        var customTestSectionName = 
            req.TestSectionName.Replace(" ", "_").ToLower();
        
        // Public Id
        var publicId = $"{testType}/{customTestTitle}/{customTestSectionName}/partition_{req.PartitionNumber}";
        
        // Upload image to cloudinary
        var result = await cloudinaryService.UploadAsync(publicId, req.File, FileType.Image);
        
        // Check whether exist secureUrl and null error message
        return string.IsNullOrEmpty(result.messageErr) && !string.IsNullOrEmpty(result.secureUrl)
        // Success
        ? Ok(new BaseResponse()
        {
            StatusCode = StatusCodes.Status200OK,
            Data = new
            {
                PublicId = result.publicId,
                Url = result.secureUrl
            }
        })
        // Error
        : BadRequest(new BaseResponse()
        {
            StatusCode = StatusCodes.Status200OK,
            Message = result.messageErr
        });
    }
    
    [HttpPost(ApiRoute.Resource.UploadAudio)]
    public async Task<IActionResult> UploadAudioAsync([FromForm] UploadAudioRequest req)
    {
        // Check validation errors
        if (!ModelState.IsValid)
            // Return [422] with validation error details
            return new ObjectResult(StatusCodes.Status422UnprocessableEntity)
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity,
                Value = ModelState
            };
        
        // Custom for public Id
        var testType = req.TestType.ToLower();
        var customTestTitle = 
            req.TestTitle.Replace(" ", "_").ToLower();
        var customTestSectionName = 
            req.TestSectionName.Replace(" ", "_").ToLower();
        
        // Public Id
        var publicId = $"{testType}/{customTestTitle}/{customTestSectionName}";
        
        // Upload audio to cloudinary
        var result = await cloudinaryService.UploadAsync(publicId, req.File, FileType.Video);
        
        // Check whether exist secureUrl and null error message
        return string.IsNullOrEmpty(result.messageErr) && !string.IsNullOrEmpty(result.secureUrl)
            // Success
            ? Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    PublicId = result.publicId,
                    Url = result.secureUrl
                }
            })
            // Error
            : BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Message = result.messageErr
            });
    }

    [HttpPost(ApiRoute.Resource.UpdateImage)]
    public async Task<IActionResult> UpdateImageAsync([FromForm] UpdateImageRequest req)
    {
        // Check validation errors
        if (!ModelState.IsValid)
            // Return [422] with validation error details
            return new ObjectResult(StatusCodes.Status422UnprocessableEntity)
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity,
                Value = ModelState
            };
        
        // Update audio to cloudinary
        var result = await cloudinaryService.UpdateAsync(req.PublicId, req.File, FileType.Image);
        
        // Check whether exist secureUrl and null error message
        return string.IsNullOrEmpty(result.messageErr) && !string.IsNullOrEmpty(result.secureUrl)
            // Success
            ? Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    PublicId = result.publicId,
                    Url = result.secureUrl
                }
            })
            // Error
            : BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Message = result.messageErr
            });
    }
    
    [HttpPost(ApiRoute.Resource.UpdateAudio)]
    public async Task<IActionResult> UpdateAudioAsync([FromForm] UpdateAudioRequest req)
    {
        // Check validation errors
        if (!ModelState.IsValid)
            // Return [422] with validation error details
            return new ObjectResult(StatusCodes.Status422UnprocessableEntity)
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity,
                Value = ModelState
            };
        
        // Update audio to cloudinary
        var result = await cloudinaryService.UpdateAsync(req.PublicId, req.File, FileType.Video);
        
        // Check whether exist secureUrl and null error message
        return string.IsNullOrEmpty(result.messageErr) && !string.IsNullOrEmpty(result.secureUrl)
            // Success
            ? Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    PublicId = result.publicId,
                    Url = result.secureUrl
                }
            })
            // Error
            : BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Message = result.messageErr
            });
    }

    [HttpDelete(ApiRoute.Resource.Delete)]
    public async Task<IActionResult> DeleteResourceAsync([FromQuery] string publicId)
    {
        var result = await cloudinaryService.DeleteAsync(publicId);
        var isDeleteSuccess = Convert.ToBoolean(result.isDeleteSucess);
        
        return string.IsNullOrEmpty(result.messageErr) && isDeleteSuccess
            // Success
            ? Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Delete resource successfully"
            })
            // Error
            : BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = result.messageErr
            });
    }
}