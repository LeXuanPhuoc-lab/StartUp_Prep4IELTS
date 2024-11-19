using System.Globalization;
using EXE202_Prep4IELTS.Attributes;
using EXE202_Prep4IELTS.Payloads;
using EXE202_Prep4IELTS.Payloads.Filters;
using EXE202_Prep4IELTS.Payloads.Responses;
using EXE202_Prep4IELTS.Payloads.Responses.VocabularyUnitSchedule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Options;
using Prep4IELTS.Business.Models;
using Prep4IELTS.Business.Services;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Business.Utils;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;
using Prep4IELTS.Data.Enum;

namespace EXE202_Prep4IELTS.Controllers;

[ApiController]
public class VocabularyUnitScheduleController(
    IVocabularyUnitScheduleService vocabScheduleService,
    IOptionsMonitor<AppSettings> monitor) : ControllerBase
{
    private readonly AppSettings _appSettings = monitor.CurrentValue;

    [HttpGet(ApiRoute.VocabularySchedule.GetDateRangeInYear, Name = nameof(GetDateRangeInYearAsync))]
    public async Task<IActionResult> GetDateRangeInYearAsync(int year)
    {
        var cultureInfo = new CultureInfo("vi-VN");
        var currentDatetime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, 
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        var dateRangeInYear = await DateTimeUtils.GetDateRangeOfWeekAsync(cultureInfo, year);
        // Initiate list date range in week response
        List<DateRangeInWeekResponse> dateRangeInYearResp = new();
        foreach (var item in dateRangeInYear)
        {
            dateRangeInYearResp.Add(new()
            {
                StartDate = item.startDate,
                EndDate = item.endDate,
                DateRangeFormat = item.dateRangeFormat
            });
        }
        
        var currentRangeInWeek = dateRangeInYearResp.FirstOrDefault(x => 
            x.StartDate <= currentDatetime && x.EndDate >= currentDatetime);
        
        return Ok(new
        {
            WeekRangeInYear = dateRangeInYearResp,
            currentWeekRange = currentRangeInWeek
        });
    }

    [ClerkAuthorize]
    [PremiumAuthorize(Types = [PremiumPackageType.Standard], AllowPremiumTrial = true)]
    [HttpGet(ApiRoute.VocabularySchedule.GetCalendar, Name = nameof(GetCalendarAsync))]
    public async Task<IActionResult> GetCalendarAsync(DateTime startDate, DateTime endDate)
    {
        // Check exist user 
        var userDto = HttpContext.Items["User"] as UserDto;
        if (userDto == null) return Unauthorized();
    
        if (startDate == DateTime.MinValue 
            || endDate == DateTime.MinValue)
        {
            var currentWeekDate = await DateTimeUtils.GetCurrentWeekDateAsync();
            startDate = currentWeekDate.startDate;
            endDate = currentWeekDate.endDate;
        }

        var vocabScheduleDtos = await vocabScheduleService.FindCalendarAsync(
            userDto.UserId, startDate, endDate);

        return Ok(new BaseResponse()
        {
            StatusCode = StatusCodes.Status200OK,
            Data = vocabScheduleDtos.ToVocabularyCalendarResponse()
        });
    }


    [ClerkAuthorize]
    [PremiumAuthorize(Types = [PremiumPackageType.Standard], AllowPremiumTrial = true)]
    [HttpGet(ApiRoute.VocabularySchedule.GetAll, Name = nameof(GetAllAsync))]
    public async Task<IActionResult> GetAllAsync([FromQuery] VocabularyScheduleFilterRequest req)
    {
        // Check exist user 
        var userDto = HttpContext.Items["User"] as UserDto;
        if (userDto == null) return Unauthorized();

        // Get all vocab schedules
        var searchTerm = req.Term;
        var vocabScheduleDtos = await vocabScheduleService.FindAllWithConditionAsync(
            // With conditions
            filter: x => x.FlashcardDetail != null && (
                x.FlashcardDetail.WordText.Contains(searchTerm) ||
                x.FlashcardDetail.WordForm.Contains(searchTerm) ||
                (x.FlashcardDetail.WordPronunciation != null 
                    && x.FlashcardDetail.WordPronunciation.Contains(searchTerm)) ||
                (x.FlashcardDetail.Description != null
                    && x.FlashcardDetail.Description.Contains(searchTerm)) && 

                // Must exist user id
                x.UserFlashcard.UserId == userDto.UserId
            ),
            // Default order by create date
            orderBy: x => x.OrderByDescending(t => t.CreateDate),
            // Include Tags
            includeProperties: "FlashcardDetail");

        // Sorting 
        if (!string.IsNullOrEmpty(req.OrderBy))
        {
            var sortingEnumerable = await vocabScheduleDtos.SortVocabularyScheduleByColumnAsync(req.OrderBy);
            vocabScheduleDtos = sortingEnumerable.ToList();
        }

        // Create paginated detail list 
        var paginatedDetail = PaginatedList<VocabularyUnitScheduleDto>.Paginate(vocabScheduleDtos,
            pageIndex: req.Page ?? 1,
            req.PageSize ?? _appSettings.PageSize);

        return !vocabScheduleDtos.Any() // Not exist any vocab schedule
            ? NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any vocab schedules."
            })
            : Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    VocabularyUnitSchdules = paginatedDetail,
                    Page = paginatedDetail.PageIndex,
                    TotalPage = paginatedDetail.TotalPage
                }
            });
    }

}