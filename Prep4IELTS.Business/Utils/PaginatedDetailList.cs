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
    
    public static PaginatedDetailList<T> CreateInstance(IEnumerable<T> source, int pageIndex, int pageSize)
    {
        // Convert to List
        var items = source.ToList();
        // Get total page 
        var totalPage = (int)Math.Ceiling(items.Count / (double)pageSize);
        // Create paging detail instance
        return new PaginatedDetailList<T>(items, pageIndex, totalPage);
    }
}