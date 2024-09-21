namespace EXE202_Prep4IELTS.Payloads.Filters;

public class UserFilterRequest
{
    public int? Page { get; set; }
    public string? Term { get; set; } = string.Empty;
    public string? Category { get; set; } = string.Empty;
    public int? PageSize { get; set; }
    // OrderBy: CreateDate, FullName
    public string? OrderBy { get; set; } = string.Empty;
    public int? RoleId { get; set; }
}