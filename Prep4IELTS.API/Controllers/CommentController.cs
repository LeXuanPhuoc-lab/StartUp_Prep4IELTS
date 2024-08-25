using EXE202_Prep4IELTS.Payloads;
using EXE202_Prep4IELTS.Payloads.Responses;
using Microsoft.AspNetCore.Mvc;
using Prep4IELTS.Business.Services.Interfaces;

namespace EXE202_Prep4IELTS.Controllers;

[ApiController]
public class CommentController(ICommentService commentService) : ControllerBase 
{
    [Route(ApiRoute.Comment.GetAllByTestId, Name = nameof(GetAllByTestIdAsync))]
    public async Task<IActionResult> GetAllByTestIdAsync([FromRoute] Guid testId, [FromQuery] int total)
    {
        // Get all by test id with particular total
        var commentDtos = await commentService.FindAllWithSizeByTestIdAsync(testId, total);

        return !commentDtos.Any()
            ? NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any comment."
            })
            : Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = commentDtos
            });
    }
}