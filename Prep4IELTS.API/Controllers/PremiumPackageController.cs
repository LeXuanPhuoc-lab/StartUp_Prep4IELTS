using EXE202_Prep4IELTS.Attributes;
using EXE202_Prep4IELTS.Payloads;
using EXE202_Prep4IELTS.Payloads.Requests.PremiumPackage;
using EXE202_Prep4IELTS.Payloads.Responses;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Prep4IELTS.Business.Constants;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace EXE202_Prep4IELTS.Controllers;

[ApiController]
public class PremiumPackageController(IPremiumPackageService premiumPackageService) : ControllerBase
{
    [HttpGet(ApiRoute.PremiumPackage.GetAll, Name = nameof(GetPremiumPackagesAsync))]
    public async Task<IActionResult> GetPremiumPackagesAsync()
    {
        var premiumPackageDtos = await premiumPackageService.FindAllAsync();
        
        return Ok(new BaseResponse()
        {
            StatusCode = StatusCodes.Status200OK,
            Data = premiumPackageDtos.Any() ? premiumPackageDtos : new List<PremiumPackageDto>()
        });
    }

    [HttpGet(ApiRoute.PremiumPackage.GetAllDraft, Name = nameof(GetPremiumPackagesDraftAsync))]
    [ClerkAuthorize(Roles = [SystemRoleConstants.Staff])]
    public async Task<IActionResult> GetPremiumPackagesDraftAsync()
    {
        var premiumPackageDtos = await premiumPackageService.FindAllDraftAsync();

        return premiumPackageDtos.Any()
            ? Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = premiumPackageDtos
            })
            : NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Not found any premium package as draft"
            });
    }
    
    [HttpPost(ApiRoute.PremiumPackage.Create, Name = nameof(CreatePremiumPackageAsync))]
    [ClerkAuthorize(Roles = [SystemRoleConstants.Staff])]
    public async Task<IActionResult> CreatePremiumPackageAsync([FromBody] CreatePremiumPackageRequest req)
    {   
        // Validations
        if(!ModelState.IsValid) return UnprocessableEntity(ModelState);
        
        // Map to premium package dto
        var premiumPackage = req.Adapt<PremiumPackage>();
        
        // Progress create new premium package
        var isCreated = await premiumPackageService.InsertAsync(premiumPackage.Adapt<PremiumPackageDto>());
        
        return isCreated
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPatch(ApiRoute.PremiumPackage.Publish, Name = nameof(PublishPremiumPackageAsync))]
    [ClerkAuthorize(Roles = [SystemRoleConstants.Staff])]
    public async Task<IActionResult> PublishPremiumPackageAsync([FromRoute] int premiumPackageId)
    {
        // Check already exist premium package
        var isAlreadyExist = await premiumPackageService.IsAlreadyPublishedAsync(premiumPackageId);
        if (isAlreadyExist)
        {
            return BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Package already published"
            });
        }
        
        // Progress publish premium package
        var isUpdated = await premiumPackageService.PublishAsync(premiumPackageId);
        
        return isUpdated
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }
    
    [HttpPatch(ApiRoute.PremiumPackage.Hidden, Name = nameof(HidePremiumPackageAsync))]
    [ClerkAuthorize(Roles = [SystemRoleConstants.Staff])]
    public async Task<IActionResult> HidePremiumPackageAsync([FromRoute] int premiumPackageId)
    {
        // Check already exist premium package
        var isAlreadyExist = await premiumPackageService.IsAlreadyPublishedAsync(premiumPackageId);
        if (!isAlreadyExist)
        {
            return BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Package already hidden"
            });
        }
        
        // Progress publish premium package
        var isUpdated = await premiumPackageService.HideAsync(premiumPackageId);
        
        return isUpdated
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPut(ApiRoute.PremiumPackage.Update, Name = nameof(UpdatePremiumPackageAsync))]
    [ClerkAuthorize(Roles = [SystemRoleConstants.Staff])]
    public async Task<IActionResult> UpdatePremiumPackageAsync([FromRoute] int premiumPackageId,[FromBody] UpdatePremiumPackageRequest req)
    {
        // Validations 
        if(!ModelState.IsValid) return UnprocessableEntity(ModelState);
        
        // Check exist premium package
        var isExistPremiumPackage = await premiumPackageService.IsExistPremiumPackageAsync(premiumPackageId);
        if (!isExistPremiumPackage)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found premium package match"
            });
        }
        
        // Check whether hidden or not 
        var isPublished = await premiumPackageService.IsAlreadyPublishedAsync(premiumPackageId);
        if (isPublished)
        {
            return BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Premium package already published. Cannot update premium package"
            });
        }
        
        // Map to dto
        var premiumPackage = req.Adapt<PremiumPackage>();
        // Set premium package id 
        premiumPackage.PremiumPackageId = premiumPackageId;
        // Progress update premium package
        await premiumPackageService.UpdateAsync(premiumPackage.Adapt<PremiumPackageDto>());
    
        // Response 
        return NoContent();
    }

    [HttpDelete(ApiRoute.PremiumPackage.Delete, Name = nameof(DeletePremiumPackageAsync))]
    [ClerkAuthorize(Roles = [SystemRoleConstants.Staff])]
    public async Task<IActionResult> DeletePremiumPackageAsync([FromRoute] int premiumPackageId, [FromQuery] bool forceDelete = false)
    {
        // Check exist premium package
        var isExistPremiumPackage = await premiumPackageService.IsExistPremiumPackageAsync(premiumPackageId);
        if (!isExistPremiumPackage)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any premium package match"
            });
        }
        
        // Check whether delete or not (only de-active) 
        var isPublished = await premiumPackageService.IsAlreadyPublishedAsync(premiumPackageId);
        if (isPublished)
        {
            return BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Premium package already published. Cannot delete premium package"
            });
        }
        
        // Check exist user premium package
        var isExistTransaction = await premiumPackageService.IsExistActiveUserPremiumPackageAsync(premiumPackageId);
        if (isExistTransaction)
        {
            return BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Premium package already exist transactions. Cannot de-active premium package"
            });
        }
        
        // Progress delete premium package (Only de-active)
        var isRemoved = !forceDelete
            // Remove by change package status
            ? await premiumPackageService.RemoveByDeActive(premiumPackageId)
            // Remove force all package relation
            : await premiumPackageService.RemoveAsync(premiumPackageId);
        
        return isRemoved // Remove successfully
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }
    
}