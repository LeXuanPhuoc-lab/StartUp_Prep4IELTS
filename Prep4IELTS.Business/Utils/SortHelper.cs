using Prep4IELTS.Business.Constants;
using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Utils;

public static class SortHelper
{
    public static async Task<IEnumerable<TestDto>> SortTestByColumnAsync(this IEnumerable<TestDto> sources, string sortPattern)
    {
        // Check is descending sorting 
        var isDescending = sortPattern.StartsWith("-");
        if (isDescending)
        {
            sortPattern = sortPattern.Trim('-');
        }

        // Define sorting pattern
        var sortMappings = new Dictionary<string, Func<TestDto, object>>()
        {
            { "TOTALENGAGED", x => x.TotalEngaged ?? null! },
            { "CREATEDATE", x => x.CreateDate }
        };
        
        // Get sorting pattern
        if (sortMappings.TryGetValue(sortPattern.ToUpper(), 
                out var sortExpression))
        {
            return await Task.FromResult(isDescending
                ? sources.OrderByDescending(sortExpression)
                : sources.OrderBy(sortExpression));
        }

        return await Task.FromResult(sources);
    }

    public static async Task<IEnumerable<UserDto>> SortUserByColumnAsync(this IEnumerable<UserDto> sources,
        string sortPattern)
    {
        var isDescending = sortPattern.StartsWith("-");
        if (isDescending)
        {
            sortPattern = sortPattern.Trim('-');
        }
        
        // Define sorting pattern
        var sortMappings = new Dictionary<string, Func<UserDto, object>>()
        {
            {"CREATEDATE", u => u.CreateDate ?? null! },
            {"FULLNAME", u => string.Join(" ", u.FirstName, u.LastName) }
        };
        
        // Get sorting pattern
        if (sortMappings.TryGetValue(sortPattern.ToUpper(), 
                out var sortExpression))
        {
            return await Task.FromResult(isDescending
                ? sources.OrderByDescending(sortExpression)
                : sources.OrderBy(sortExpression));
        }

        return await Task.FromResult(sources);
    }
    
    public static async Task<IEnumerable<SpeakingSampleDto>> SortSpeakingSampleByColumnAsync(this IEnumerable<SpeakingSampleDto> sources,
        string sortPattern)
    {
        var isDescending = sortPattern.StartsWith("-");
        if (isDescending)
        {
            sortPattern = sortPattern.Trim('-');
        }
        
        // Define sorting pattern
        var sortMappings = new Dictionary<string, Func<SpeakingSampleDto, object>>()
        {
            {"CREATEDATE", u => u.CreateDate },
        };
        
        // Get sorting pattern
        if (sortMappings.TryGetValue(sortPattern.ToUpper(), 
                out var sortExpression))
        {
            return await Task.FromResult(isDescending
                ? sources.OrderByDescending(sortExpression)
                : sources.OrderBy(sortExpression));
        }

        return await Task.FromResult(sources);
    }
    
    public static async Task<IEnumerable<FlashcardDto>> SortFlashcardByColumnAsync(this IEnumerable<FlashcardDto> sources,
        string sortPattern)
    {
        var isDescending = sortPattern.StartsWith("-");
        if (isDescending)
        {
            sortPattern = sortPattern.Trim('-');
        }
        
        // Define sorting pattern
        var sortMappings = new Dictionary<string, Func<FlashcardDto, object>>()
        {
            {"CREATEDATE", u => u.CreateDate ?? null! },
            {"TOTALVIEW", u => u.TotalView ?? null! },
            {"TOTALWORD", u => u.TotalWords ?? null! },
        };
        
        // Get sorting pattern
        if (sortMappings.TryGetValue(sortPattern.ToUpper(), 
                out var sortExpression))
        {
            return await Task.FromResult(isDescending
                ? sources.OrderByDescending(sortExpression)
                : sources.OrderBy(sortExpression));
        }

        return await Task.FromResult(sources);
    }

    public static async Task<IEnumerable<FlashcardExamHistoryDto>> SortFlashcardExamByColumnAsync(this IEnumerable<FlashcardExamHistoryDto> sources,
        string sortPattern)
    {
        var isDescending = sortPattern.StartsWith("-");
        if (isDescending)
        {
            sortPattern = sortPattern.Trim('-');
        }

        // Define sorting pattern
        var sortMappings = new Dictionary<string, Func<FlashcardExamHistoryDto, object>>()
        {
            {"CREATEDATE", u => u.TakenDate },
        };

        // Get sorting pattern
        if (sortMappings.TryGetValue(sortPattern.ToUpper(),
                out var sortExpression))
        {
            return await Task.FromResult(isDescending
                ? sources.OrderByDescending(sortExpression)
                : sources.OrderBy(sortExpression));
        }

        return await Task.FromResult(sources);
    }

    public static async Task<IEnumerable<VocabularyUnitScheduleDto>> SortVocabularyScheduleByColumnAsync(this IEnumerable<VocabularyUnitScheduleDto> sources,
        string sortPattern)
    {
        var isDescending = sortPattern.StartsWith("-");
        if (isDescending)
        {
            sortPattern = sortPattern.Trim('-');
        }

        // Define sorting pattern
        var sortMappings = new Dictionary<string, Func<VocabularyUnitScheduleDto, object>>()
        {
            {"CREATEDATE", u => u.CreateDate },
            {"ALPHABET", u => u.FlashcardDetail.WordText },
            {"MONDAY", u => u.Weekday != null && u.Weekday.Any(x => x.Equals(WeekDayConstants.Monday.ToString())) },
            {"TUESDAY", u => u.Weekday != null && u.Weekday.Any(x => x.Equals(WeekDayConstants.Tuesday.ToString())) },
            {"WEDNESDAY", u => u.Weekday != null && u.Weekday.Any(x => x.Equals(WeekDayConstants.Wednesday.ToString())) },
            {"THURSDAY", u => u.Weekday != null && u.Weekday.Any(x => x.Equals(WeekDayConstants.Thursday.ToString())) },
            {"FRIDAY", u => u.Weekday != null && u.Weekday.Any(x => x.Equals(WeekDayConstants.Friday.ToString())) },
            {"SATURDAY", u => u.Weekday != null && u.Weekday.Any(x => x.Equals(WeekDayConstants.Saturday.ToString())) },
            {"SUNDAY", u => u.Weekday != null && u.Weekday.Any(x => x.Equals(WeekDayConstants.Sunday.ToString())) }
        };

        // Get sorting pattern
        if (sortMappings.TryGetValue(sortPattern.ToUpper(),
                out var sortExpression))
        {
            return await Task.FromResult(isDescending
                ? sources.OrderByDescending(sortExpression)
                : sources.OrderBy(sortExpression));
        }

        return await Task.FromResult(sources);
    }

	public static async Task<IEnumerable<TransactionDto>> SortTransactionByColumnAsync(this IEnumerable<TransactionDto> sources,
		string sortPattern)
	{
		var isDescending = sortPattern.StartsWith("-");
		if (isDescending)
		{
			sortPattern = sortPattern.Trim('-');
		}

		// Define sorting pattern
		var sortMappings = new Dictionary<string, Func<TransactionDto, object>>()
		{
			{"PAYMENTAMOUNT", u => u.PaymentAmount },
			{"CREATEAT", u => u.CreateAt },
			{"TRANSACTIONDATE", u => u.TransactionDate ?? null! },
			{"TRANSACTIONSTATUS", u => u.TransactionStatus },
			{"TRANSACTIONCODE", u => u.TransactionCode },
			{"CANCELLATIONREASON", u => u.CancellationReason ?? null!},
			{"CANCELLEDAT", u => u.CancelledAt ?? null!},
		};

		// Get sorting pattern
		if (sortMappings.TryGetValue(sortPattern.ToUpper(),
				out var sortExpression))
		{
			return await Task.FromResult(isDescending
				? sources.OrderByDescending(sortExpression)
				: sources.OrderBy(sortExpression));
		}

		return await Task.FromResult(sources);
	}

}