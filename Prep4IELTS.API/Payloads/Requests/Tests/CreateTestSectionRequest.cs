using System.ComponentModel.DataAnnotations;
using Prep4IELTS.Business.Validations;

namespace EXE202_Prep4IELTS.Payloads.Requests.Tests;

public class CreateTestSectionRequest
{
     [StringLength(maximumLength: 50, MinimumLength = 5, ErrorMessage = "The test section name must be between 5 and 50 characters long.")]
     [Required(ErrorMessage = "Test title is required.")]
     public string TestSectionName { get; set; } = null!;
     
     public string? ReadingDesc { get; set; }
     
     [Required(ErrorMessage = "Section total question is required.")]
     public int TotalQuestion { get; set; }

     public string? SectionTranscript { get; set; }

     public CloudResourceRequest? CloudResource { get; set; } 
     
     [Required(ErrorMessage = "Section partitions is required.")]
     public List<CreateTestSectionPartitionRequest> TestSectionPartitions { get; set; } = new();
}