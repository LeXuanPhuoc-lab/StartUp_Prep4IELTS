using System.ComponentModel.DataAnnotations;
using Prep4IELTS.Business.Validations;

namespace EXE202_Prep4IELTS.Payloads.Requests.Tests;

public class CreateTestSectionPartitionRequest
{
    [Required(ErrorMessage = "Partition description is required.")]
    public string PartitionDesc { get; set; } = null!;
    
    // public string? PublicIdForImageResource { get; set; }
    
    // [MaxLength(2048, ErrorMessage = "Image just allowed maximum 2048 characters long.")]
    // public string? PartitionImage { get; set; }
    
    public CloudResourceRequest? CloudResource { get; set; } 
    
    [Required(ErrorMessage = "Partition tag is required.")]
    public int PartitionTagId { get; set; }

    [Required(ErrorMessage = "Questions for partition is required.")]
    public List<CreateQuestionRequest> Questions { get; set; } = new();
}