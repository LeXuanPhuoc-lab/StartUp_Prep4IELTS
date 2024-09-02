using System.ComponentModel.DataAnnotations;

namespace EXE202_Prep4IELTS.Payloads.Requests.Tests;

public class CloudResourceRequest
{
    public string? PublicId { get; set; } = string.Empty;
     
    [MaxLength(2048, ErrorMessage = "Audio resource just allowed maximum 2048 characters long.")]
    public string? Url { get; set; }
}