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

        var userService = context.RequestServices.GetRequiredService<IUserService>();

        var user = await userService.GetUserByClerkId(clerkId);

        if (user == null)
        {
            await _next(context);
            return;
        }

        context.Items["User"] = user;
        await _next(context);
    }
}