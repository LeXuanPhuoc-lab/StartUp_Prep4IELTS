using System.ComponentModel.DataAnnotations;

namespace EXE202_Prep4IELTS.Payloads.Requests.Tests;

public class UpdateTestRequest
{
    [StringLength(maximumLength: 155, MinimumLength = 10, ErrorMessage = "The test title must be between 10 and 155 characters long.")]
    [Required(ErrorMessage = "Test title is required.")]
    public string TestTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "Test duration is required.")]
    public int Duration { get; set; }

    [MaxLength(50, ErrorMessage = "The test type only allowed maximum 50 characters long.")]
    [Required(ErrorMessage = "Test type is required.")]
    public string TestType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Test category is required.")]
    public int TestCategoryId { get; set; }
    
    public List<int>? Tags { get; set; } = new();
}