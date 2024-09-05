using EXE202_Prep4IELTS.Payloads;
using EXE202_Prep4IELTS.Payloads.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Prep4IELTS.Business.Models;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Business.Utils;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace EXE202_Prep4IELTS.Controllers;

[ApiController]
public class CommentController(
    ICommentService commentService,
    IOptionsMonitor<AppSettings> monitor) : ControllerBase
{
    private readonly AppSettings _appSettings = monitor.CurrentValue;
    
    [Route(ApiRoute.Comment.GetAllByTestId, Name = nameof(GetAllByTestIdAsync))]
    public async Task<IActionResult> GetAllByTestIdAsync([FromRoute] Guid testId, int? pageSize, int? page = 1)
    {
        // Get all by test id with particular total
        var commentDtos = await commentService.FindAllWithConditionAndPagingAsync(
            // With conditions
            filter: cmt => cmt.TestId.Equals(testId),
            // order descending by comment date
            orderBy: new (cmt => cmt.OrderByDescending(x => x.CommentDate)),
            // Include nothing
            includeProperties: null,
            // Get with particular pageIndex
            pageIndex: page,
            // With pageSize elements
            pageSize: pageSize ?? _appSettings.PageSize);

        // Total actual tests
        var actualTotal = await commentService.CountTotalByTestId(testId);
        
        // Create paginated detail list 
        var paginatedDetail = PaginatedDetailList<CommentDto>.CreateInstance(commentDtos,
            pageIndex: page ?? 1,
            pageSize ?? _appSettings.PageSize,
            actualItem: actualTotal);
        
        return !commentDtos.Any()
            ? NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any comment."
            })
            : Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    Comments = paginatedDetail,
                    Page = paginatedDetail.PageIndex,
                    TotalPage = paginatedDetail.TotalPage
                }
            });
    }
}