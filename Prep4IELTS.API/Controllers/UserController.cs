using EXE202_Prep4IELTS.Attributes;
using EXE202_Prep4IELTS.Payloads;
using Microsoft.AspNetCore.Mvc;
using Prep4IELTS.Data.Dtos;

namespace EXE202_Prep4IELTS.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    [ClerkAuthorize]
    [HttpGet(ApiRoute.User.WhoAmI, Name = nameof(WhoAmI))]
    public IActionResult WhoAmI()
    {
        var user = (UserDto?)HttpContext.Items["User"];
        return Ok(user);
    }
}