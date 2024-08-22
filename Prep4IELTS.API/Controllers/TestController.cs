using EXE202_Prep4IELTS.Payloads;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data;

namespace EXE202_Prep4IELTS.Controllers;

[ApiController]
public class TestController(UnitOfWork unitOfWork) : ControllerBase
{
    
    [Route(ApiRoute.Test.GetAll)]
    public async Task<IActionResult> GetAllTestAsync()
    {
        return Ok(await unitOfWork.TestRepository.FindAllAsync());
    }
}