namespace Prep4IELTS.Business.Models;

public class AppSettings
{
    public int PageSize { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public string AudioPath { get; set; } = string.Empty;
    public string CloudinaryUrl { get; set; } = string.Empty;
}