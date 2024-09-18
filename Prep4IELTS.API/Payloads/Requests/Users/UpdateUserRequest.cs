using System.ComponentModel.DataAnnotations;

namespace EXE202_Prep4IELTS.Payloads.Requests.Users;

public class UpdateUserRequest
{
    [StringLength(maximumLength: 50, MinimumLength = 8, ErrorMessage = "Username length must be between 8 and 50 characters.")]
    public string? Username { get; set; } = string.Empty;
    
    [MaxLength(length: 50, ErrorMessage = "FirstName length less than 50 characters.")]
    public string? FirstName { get; set; } = string.Empty;
    
    [MaxLength(length: 50, ErrorMessage = "LastName length less than 50 characters.")]
    public string? LastName { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }
    
    [Phone]
    public string? Phone { get; set; }
    
    public DateTime? TestTakenDate { get; set; }

    public string? TargetScore { get; set; }
}