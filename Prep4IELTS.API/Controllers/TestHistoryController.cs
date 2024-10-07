using EXE202_Prep4IELTS.Attributes;
using EXE202_Prep4IELTS.Payloads;
using EXE202_Prep4IELTS.Payloads.Responses;
using EXE202_Prep4IELTS.Payloads.Responses.Tests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Prep4IELTS.Business.Constants;
using Prep4IELTS.Business.Models;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Business.Utils;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Enum;

namespace EXE202_Prep4IELTS.Controllers;

[ApiController]
public class TestHistoryController(
    ITestHistoryService testHistoryService,
    ITestService testService,
    ITestPartitionHistoryService testPartitionHistoryService,
    IUserPremiumPackageService userPremiumPackageService,
    IOptionsMonitor<AppSettings> monitor) : ControllerBase
{
    private readonly AppSettings _appSettings = monitor.CurrentValue;
    
    //  Summary:
    //      Get all history 
    [HttpGet(ApiRoute.TestHistory.GetAllByUserId, Name = nameof(GetAllHistoryByUserIdAsync))]
    public async Task<IActionResult> GetAllHistoryByUserIdAsync(Guid userId, int? page, int? pageSize)
    {
        // Get all user history
        var testHistoryDtos = await testHistoryService.FindAllUserIdAsync(userId);

        // Initiate test response including test history
        var listTestResp = testHistoryDtos.ToList().ToListTestResponse();
        
        // Paging 
        var testHistoryPagingList =
            PaginatedList<TestResponse>.Paginate(
                source: listTestResp,
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
                    UserTests = testHistoryPagingList,
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
        var testHistoryDto = await testHistoryService.FindByIdWithIncludePartitionAndGradeAsync(id);
        
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
        
        // Section resources
        var sectionResources = testHistoryDto.Test.TestSections
            .Select(ts => ts.CloudResource)
            .ToList();
        // Test section 
        var sections = testHistoryDto.Test.TestSections.ToList();
        // Foreach section history, add cloud resource
        for (int i = 0; i < groupedSectionHistories.Count; i++)
        {
            if (sectionResources[i] != null!) // Ensure exist resource as same index as section history
            {
                // Assign cloud resource for section history
                groupedSectionHistories[i].CloudResource = sectionResources[i];
            }
            // SectionId
            var sectionId = sections[i].TestSectionId;

            // Assign test section id
            groupedSectionHistories[i].TestSectionId = sectionId;
            // Get section transcript 
            groupedSectionHistories[i].Transcript = 
                await testService.FindTestSectionTranscriptAsync(sectionId);
        }
        
        
        // Init test history detail response
        TestHistoryResponse testHistoryResp = new() { TestHistory = testHistoryDto };
        // Calc total right, wrong, skip foreach test section
        testHistoryResp.SectionHistories = groupedSectionHistories;
        
        // Clear partition histories within TestHistory
        testHistoryDto.PartitionHistories.Clear();
        // Clear test sections in test history
        testHistoryDto.Test.TestSections.Clear();
        
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
    [ClerkAuthorize(Roles = [SystemRoleConstants.Staff, SystemRoleConstants.Student])]
    public async Task<IActionResult> GetPartitionHistoryWithGradeByIdAsync([FromRoute] int partitionId, [FromRoute] int testGradeId)
    {
        // User
        var userDto = HttpContext.Items["User"] as UserDto;
        if(userDto == null) return Unauthorized();
        // Check for premium package
        var userPremiumPackage = await userPremiumPackageService.FindUserPremiumPackageAsync(userDto.UserId);
        // Has premium package and is activated
        bool hasPremiumPackage = userPremiumPackage != null 
            && (userPremiumPackage.IsActive || userPremiumPackage.TotalTrials > 0);
        
        var partitionHistoryDto = await testPartitionHistoryService.FindByIdAndGradeAsync(partitionId, testGradeId, hasPremiumPackage);
        
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