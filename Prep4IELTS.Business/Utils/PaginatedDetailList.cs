namespace Prep4IELTS.Business.Utils;

public class PaginatedDetailList<T> : List<T>
{
    public int PageIndex { get; set; }
    public int TotalPage { get; set; }

    private PaginatedDetailList(List<T> items, int pageIndex, int totalPage)
    {
        PageIndex = pageIndex;
        TotalPage = totalPage;
        AddRange(items);
    }
    
    public static PaginatedDetailList<T> CreateInstance(IEnumerable<T> source, int pageIndex, int pageSize, int actualItem)
    {
        // Convert source to List of item
        var items = source.ToList();
        
        // Ensure valid pageSize
        if(pageSize < 1) pageSize = items.Count;
        
        // Count total page
        var totalPage = (int)Math.Ceiling(actualItem / (double)pageSize);
        
        // Ensure valid page index
        if (pageIndex < 1 || pageIndex > totalPage) pageIndex = 1;

        // Create paging detail instance
        return new PaginatedDetailList<T>(items, pageIndex, totalPage);
    }
}