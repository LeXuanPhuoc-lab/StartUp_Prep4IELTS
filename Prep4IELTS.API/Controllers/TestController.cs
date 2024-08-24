using System.Net;
using System.Web;
using EXE202_Prep4IELTS.Payloads;
using EXE202_Prep4IELTS.Payloads.Responses;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;
using Prep4IELTS.Business.Models;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Business.Utils;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace EXE202_Prep4IELTS.Controllers;

[ApiController]
public class TestController(
    ITestService testService,
    ITestCategoryService testCategoryService,
    IMapper mapper,
    IOptionsMonitor<AppSettings> monitor) : ControllerBase
{
    private readonly AppSettings _appSettings = monitor.CurrentValue;

    //  Summary:
    //      Get all test categories that use to filter test by category
    [Route(ApiRoute.TestCategory.GetAll, Name = nameof(GetAllTestCategoryAsync))]
    public async Task<IActionResult> GetAllTestCategoryAsync()
    {
        // Get all test category
        var testCategoryDtos = await testCategoryService.FindAllAsync();

        return !testCategoryDtos.Any() // Not exist any test category
            ? NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any tests."
            })
            : Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = testCategoryDtos
            });
    }

    //  Summary:
    //      Get all existing tests 
    [Route(ApiRoute.Test.GetAll, Name = nameof(GetAllTestAsync))]
    public async Task<IActionResult> GetAllTestAsync(
        int? page, string? term, string? userId, string? category, int? pageSize)
    {
        // Get all test
        var testDtos = await testService.FindAllWithConditionAndPagingAsync(
            // With conditions
            // Search with test title
            filter: x => x.TestTitle.Contains(term ?? "") &&
                 // filter with test category name
                 (string.IsNullOrEmpty(category) ||
                  x.TestCategory.TestCategoryName!.Equals(category.Replace("%", " "))),
            // Order Descending by total engagement
            orderBy: x => x.OrderByDescending(tst => tst.TotalEngaged),
            // Include Tags
            includeProperties: "Tags",
            // With page index
            pageIndex:page,
            // With page size
            pageSize: pageSize ?? _appSettings.PageSize,
            // Include user test histories (if any)
            userId);
        
        // Total actual tests
        var actualTotal = await testService.CountTotalAsync();
        
        // Create paginated detail list 
        var paginatedDetail = PaginatedDetailList<TestDto>.CreateInstance(testDtos, 
            pageIndex: page ?? 1, 
            actualItem: actualTotal);

        return !testDtos.Any() // Not exist any test
            ? NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any tests."
            })
            : Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    Tests = paginatedDetail, 
                    Page = paginatedDetail.PageIndex, 
                    TotalPage = paginatedDetail.TotalPage
                }
            });
    }

    //  Summary:
    //      Get test by id 
    [Route(ApiRoute.Test.GetById, Name = nameof(GetByIdAsync))]
    public async Task<IActionResult> GetByIdAsync(int id, string? userId)
    {
        // Get by id 
        var testDto = await testService.FindByIdAsync(id, 
            userId != null ? Guid.Parse(userId) : null!);

        return testDto == null!
            ? NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Not found any test match id {id}"
            })
            : Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = testDto
            });
    }
}