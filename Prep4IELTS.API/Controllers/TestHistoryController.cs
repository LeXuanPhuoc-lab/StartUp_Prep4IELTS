using EXE202_Prep4IELTS.Payloads;
using EXE202_Prep4IELTS.Payloads.Responses;
using EXE202_Prep4IELTS.Payloads.Responses.Tests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Prep4IELTS.Business.Models;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Business.Utils;
using Prep4IELTS.Data.Dtos;

namespace EXE202_Prep4IELTS.Controllers;

[ApiController]
public class TestHistoryController(
    ITestHistoryService testHistoryService,
    ITestPartitionHistoryService testPartitionHistoryService,
    IOptionsMonitor<AppSettings> monitor) : ControllerBase
{
    private readonly AppSettings _appSettings = monitor.CurrentValue;
    
    //  Summary:
    //      Get all history 
    [HttpGet(ApiRoute.TestHistory.GetAllByUserId, Name = nameof(GetAllHistoryByUserIdAsync))]
    public async Task<IActionResult> GetAllHistoryByUserIdAsync(Guid userId, int? page, int? pageSize)
    {
        var testHistoryDtos = await testHistoryService.FindAllUserIdAsync(userId);

        // Paging 
        var testHistoryPagingList =
            PaginatedList<TestHistoryDto>.Paginate(
                source: testHistoryDtos,
                pageIndex: page ?? 1,
                pageSize: pageSize ?? _appSettings.PageSize);

        return !testHistoryPagingList.Any()
            ? NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any history"
            })
            : Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    TestHistories = testHistoryPagingList,
                    PageIndex = testHistoryPagingList.PageIndex,
                    TotalPage = testHistoryPagingList.TotalPage
                }
            });
    }

    //  Summary:
    //      Get history by id
    [HttpGet(ApiRoute.TestHistory.GetHistoryById, Name = nameof(GetHistoryByIdAsync))]
    public async Task<IActionResult> GetHistoryByIdAsync([FromRoute] int id)
    {
        // Get test history by id, then include partition history and test grade 
        var testHistoryDto = await testHistoryService.FindByIdWithIncludePartitionAndGrade(id);
        
        // Check exist test history
        if (testHistoryDto == null!)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any history match"
            });
        }
        
        // Group partition history with unique TestSectionName, then map to IDictionary<string, List<PartitionHistoryDto>>
        var groupedSectionHistories = testHistoryDto.PartitionHistories
            .GroupBy(x => x.TestSectionName)
            .Select(g => new SectionHistoryResponse().CalculateTotal(
                sectionName: g.Key,
                totalQuestion: testHistoryDto.TotalQuestion,
                partitionHistories: g.Select(ph => ph).ToList())).ToList();
        
        // Clear elements within TestHistory
        testHistoryDto.PartitionHistories.Clear();
        
        // Init test history detail response
        TestHistoryResponse testHistoryResp = new() { TestHistory = testHistoryDto };
        // Calc total right, wrong, skip foreach test section
        testHistoryResp.SectionHistories = groupedSectionHistories;
        
        return testHistoryDto == null!
            ? NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any history match"
            })
            : Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = testHistoryResp
            });
    }

    //  Summary:
    //      Get detail for particular grade
    [HttpGet(ApiRoute.TestHistory.GetPartitionHistoryWithGradeById, Name = nameof(GetPartitionHistoryWithGradeByIdAsync))]
    public async Task<IActionResult> GetPartitionHistoryWithGradeByIdAsync([FromRoute] int partitionId, [FromRoute] int testGradeId)
    {
        var partitionHistoryDto = await testPartitionHistoryService.FindByIdAndGradeAsync(partitionId, testGradeId);
        
        return partitionHistoryDto == null!
            ? NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any partition history match"
            })
            : Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = partitionHistoryDto
            });
    }
}