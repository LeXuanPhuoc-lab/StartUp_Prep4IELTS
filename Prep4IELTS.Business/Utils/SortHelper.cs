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
}