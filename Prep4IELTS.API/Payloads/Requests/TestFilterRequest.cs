namespace EXE202_Prep4IELTS.Payloads.Requests;

public class TestFilterRequest
{
    public int? Page { get; set; }
    public string? Term { get; set; } = string.Empty;
    public string? Category { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
    public int? PageSize { get; set; }
    // OrderBy: TotalEngaged, CreateDate
    public string? OrderBy { get; set; } = string.Empty;
}