namespace EXE202_Prep4IELTS.Payloads.Filters;

public class SpeakingSampleFilterRequest
{
    public int? Page { get; set; }
    public string? Term { get; set; } = string.Empty;
    public int? PageSize { get; set; }
    // OrderBy: CreateDate
    public string? OrderBy { get; set; } = string.Empty;
}