namespace EXE202_Prep4IELTS.Payloads.Filters;

public class FlashcardFilterRequest
{
    public int? Page { get; set; }
    public string? Term { get; set; } = string.Empty;
    public int? PageSize { get; set; }
    // OrderBy: TotalView, CreateDate
    public string? OrderBy { get; set; } = string.Empty;
}