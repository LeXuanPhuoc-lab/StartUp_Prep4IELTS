using System.Security.Claims;
using Prep4IELTS.Business.Services.Interfaces;

namespace EXE202_Prep4IELTS.Middlewares;

public class ClerkAuthMiddleware
{
    private readonly RequestDelegate _next;

    public ClerkAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var clerkId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(clerkId))
        {
            await _next(context);
            return;
        }

        // Inject services
        var userService = context.RequestServices.GetRequiredService<IUserService>();
        var userPremiumPackageService = context.RequestServices.GetRequiredService<IUserPremiumPackageService>();
        
        // Find user by clerkId
        var user = await userService.GetUserByClerkId(clerkId);
        
        // Check exist user 
        if (user == null)
        {
            await _next(context);
            return;
        }

        // Find user premium package
        var userPremiumPackage = await userPremiumPackageService.FindUserPremiumPackageAsync(user.UserId, clerkId);
        
        if(userPremiumPackage != null)// Check exist user premium 
            // Add key-value pair to items dictionary of HttpContext
            context.Items["UserPremiumPackage"] = userPremiumPackage;
        
        context.Items["User"] = user;
        await _next(context);
    }
}