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
}