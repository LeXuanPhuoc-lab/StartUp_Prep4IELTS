using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Prep4IELTS.Data.Dtos;

namespace EXE202_Prep4IELTS.Attributes;

public class ClerkAuthorizeAttribute : Attribute, IActionFilter
{
    public string[] Roles { get; set; } = [];
    public bool Require { get; set; } = true;

    public void OnActionExecuting(ActionExecutingContext context)
    {
        //user sẽ được tìm thấy nếu token hợp lệ và user tồn tại trong DB sau khi đã chạy ClerkAuthMiddleware đã được config trong pipeline ở Program
        var user = (UserDto?)context.HttpContext.Items["User"];

        if (user == null)
        {
            if (!Require) return; //không bắt buộc có user (token)

            //bắt buộc phải có và trả về 401
            context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
            return;
        }

        //Roles rỗng nghĩa là request user có role gì cũng OK, không cần check
        if (Roles.IsNullOrEmpty()) return;

        if (!Roles.Contains(user.Role?.RoleName))
        {
            //role không phù hợp, Response 403
            context.Result = new Microsoft.AspNetCore.Mvc.ForbidResult();
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}