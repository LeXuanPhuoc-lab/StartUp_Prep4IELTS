using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Prep4IELTS.Business.Constants;
using Prep4IELTS.Business.Services;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Enum;
using Prep4IELTS.Data.Extensions;

namespace EXE202_Prep4IELTS.Attributes;

public class PremiumAuthorizeAttribute : System.Attribute, IActionFilter
{
    public PremiumPackageType[] Types = [];
    public bool AllowPremiumTrial = false;
    public bool UpdateTrialValue = false;
    
    // Called before action executes, after binding success
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Check if premium package is required
        if (!Types.Any()) return;
        
        // Check premium package item exist in context
        var userPremiumPackage = context.HttpContext.Items["UserPremiumPackage"] as UserPremiumPackageDto;
        
        // Not exist premium package
        if (userPremiumPackage == null)
        {
            // Set result to Forbid
            context.Result = new ForbidResult();
            return;
        }
        
        // Check if not allow premium trial 
        if (!userPremiumPackage.IsActive && !AllowPremiumTrial)
        {
            // Set result to Forbid
            context.Result = new ForbidResult();
            return;
        }
        
        // Check for premium trials threshold
        if (!userPremiumPackage.IsActive 
            && userPremiumPackage.TotalTrials == 0)
        {
            // Set result to Forbid
            context.Result = new ForbidResult();
            return;
        }
        
        // Get user premium package service
        var userPremiumPackageService = context.HttpContext.RequestServices.GetRequiredService<IUserPremiumPackageService>();
        
        // If access as premium trial, subtract total trial
        if (!userPremiumPackage.IsActive
            && userPremiumPackageService != null!
            // Is update trial value
            && UpdateTrialValue)
        {
            Task.Run(async () => await userPremiumPackageService.UpdateUserPremiumPackageTrialAsync(
                userPremiumPackage.User.UserId));
        }
        
        // Handle action permission for each level
        switch (userPremiumPackage.PremiumPackage.PackageType)
        {
            case PremiumPackageConstants.Basic:
                break;
            case PremiumPackageConstants.Medium:
                // Check if higher premium level
                if (Types.Any(ppt => ppt == PremiumPackageType.Basic)) return;
                break;
            case PremiumPackageConstants.Standard:
                return; // Allow access to all function
        }
        
        // Check valid user premium access
        bool allowToAccessAction = 
            Types.Any(ppt => 
                ppt.GetDescription().Equals(userPremiumPackage.PremiumPackage.PackageType));
        
        // Premium Authorized success and user premium package trials still > 0
        if (allowToAccessAction 
            || userPremiumPackage.TotalTrials > 0) return;
        
        // Not allow to access action
        context.Result = new ForbidResult();
    }

    // Called after action executes, after action result
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}