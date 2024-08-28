namespace Prep4IELTS.Business.Utils;

public class PaginatedList<T> : List<T>
{
    public int PageIndex { get; set; }
    public int TotalPage { get; set; }

    private PaginatedList(List<T> items, int pageIndex, int totalPage)
    {
        PageIndex = pageIndex;
        TotalPage = totalPage;
        AddRange(items);
    }
    
    public static PaginatedList<T> Paginate(IEnumerable<T> source, int pageIndex, int pageSize)
    {
        // Convert to List
        var items = source.ToList();

        var totalPage = (int)Math.Ceiling(items.Count / (double)pageSize);

        if (pageIndex < 1 || pageIndex > totalPage) pageIndex = 1;

        items = items.Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        
        // Create paging detail instance
        return new PaginatedList<T>(items, pageIndex, totalPage);
    }
}