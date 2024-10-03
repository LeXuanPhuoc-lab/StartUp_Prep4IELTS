using System.Globalization;
using EXE202_Prep4IELTS.Attributes;
using EXE202_Prep4IELTS.Payloads;
using EXE202_Prep4IELTS.Payloads.Responses;
using EXE202_Prep4IELTS.Payloads.Responses.VocabularyUnitSchedule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Business.Utils;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Enum;

namespace EXE202_Prep4IELTS.Controllers;

[ApiController]
public class VocabularyUnitScheduleController(
    IVocabularyUnitScheduleService vocabScheduleService) : ControllerBase
{
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

        var vocabScheduleDtos = await vocabScheduleService.GetCalendarAsync(
            userDto.UserId, startDate, endDate);

        return Ok(new BaseResponse()
        {
            StatusCode = StatusCodes.Status200OK,
            Data = vocabScheduleDtos.ToVocabularyCalendarResponse()
        });
    }
}