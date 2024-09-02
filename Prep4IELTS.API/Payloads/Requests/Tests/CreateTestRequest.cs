using System.ComponentModel.DataAnnotations;
using Prep4IELTS.Data.Enum;

namespace EXE202_Prep4IELTS.Payloads.Requests.Tests;

public class CreateTestRequest
{
    [StringLength(maximumLength: 155, MinimumLength = 10, ErrorMessage = "The test title must be between 10 and 155 characters long.")]
    [Required(ErrorMessage = "Test title is required.")]
    public string TestTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "Test duration is required.")]
    public int Duration { get; set; }

    [MaxLength(50, ErrorMessage = "The test type only allowed maximum 50 characters long.")]
    [Required(ErrorMessage = "Test type is required.")]
    public string TestType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Total question is required.")]
    public int TotalQuestion { get; set; } 
    
    [Required(ErrorMessage = "Total section is required.")]
    public int TotalSection { get; set; } 
    
    [Required(ErrorMessage = "Test category is required.")]
    public int TestCategoryId { get; set; }

    [StringLength(maximumLength: 155, MinimumLength = 2,
        ErrorMessage = "Create by must be between 2 and 155 characters long.")]

    public string? CreateBy { get; set; } = null!;
    
    public List<int>? Tags { get; set; } = new();

    public List<CreateTestSectionRequest> TestSections { get; set; } = new();
}