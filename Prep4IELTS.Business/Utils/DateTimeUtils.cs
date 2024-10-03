using System.Globalization;
using System.Security.Cryptography;

namespace Prep4IELTS.Business.Utils;

public static class DateTimeUtils
{
    private static async Task<DateTime> GetNextMondayAsync(DateTime datetime)
    {        
        return await Task.FromResult(
            datetime.AddDays((7 - (int)datetime.DayOfWeek + (int)DayOfWeek.Monday) % 7));
    }

    public static async Task<string> GetWeekAsync(DateTime currentDateTime) 
    {
        var start = await GetNextMondayAsync(currentDateTime);
        var end = start.AddDays(6);
        return await Task.FromResult(start.ToShortDateString() + " - " + end.ToShortDateString());
    }
    
    public static async Task<IList<(DateTime startDate, DateTime endDate, string dateRangeFormat)>> 
        GetDateRangeOfWeekAsync(CultureInfo cultureInfo, int year)
    {
        var lastDateOfYear = new DateTime(year, 12, 31);
        var firstDate = new DateTime(year, 1, 1);
        var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;

        while (firstDate.DayOfWeek != firstDayOfWeek)
        {
            // firstDate = firstDate.AddDays(1);
            firstDate = firstDate.Subtract(TimeSpan.FromDays(1));
        }
        
        var numberOfWeeksInYear = cultureInfo.Calendar.GetWeekOfYear(lastDateOfYear, 
            cultureInfo.DateTimeFormat.CalendarWeekRule, firstDayOfWeek);

        var dateRangeInWeekResp = new List<(DateTime startDate, DateTime endDate, string)>();

        var currentDate = firstDate;
        for (int i = 0; i < numberOfWeeksInYear; i++)
        {
            var lastDateInWeek = currentDate.AddDays(6);
            
            if (lastDateInWeek.Year == year)
            {
                dateRangeInWeekResp.Add((currentDate, lastDateInWeek, $"{currentDate:dd/MM}-{lastDateInWeek:dd/MM}"));
                // Monday of week later
                currentDate = lastDateInWeek.AddDays(1);
            }
        }

        return await Task.FromResult(dateRangeInWeekResp);
    }

    public static async Task<(DateTime startDate, DateTime endDate)> GetCurrentWeekDateAsync()
    {
        DateTime currentDatetime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, 
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        DateTime startDate = currentDatetime;
        DateTime endDate = DateTime.MinValue;
        
        while (startDate.DayOfWeek != DayOfWeek.Monday)
        {
            startDate = startDate.Subtract(TimeSpan.FromDays(1));
        }
        endDate = startDate.AddDays(6);
    
        return await Task.FromResult((startDate, endDate));
    }
    
    public static (DateTime startDate, DateTime endDate) GetCurrentWeekDate()
    {
        DateTime currentDatetime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, 
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        DateTime startDate = currentDatetime;
        DateTime endDate = DateTime.MinValue;
        
        while (startDate.DayOfWeek != DayOfWeek.Monday)
        {
            startDate = startDate.Subtract(TimeSpan.FromDays(1));
        }
        endDate = startDate.AddDays(6);

        return (startDate, endDate);
    }
    
}